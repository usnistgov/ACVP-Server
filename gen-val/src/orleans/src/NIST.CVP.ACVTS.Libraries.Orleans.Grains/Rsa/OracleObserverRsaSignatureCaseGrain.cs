using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
{
    public class OracleObserverRsaSignatureCaseGrain : ObservableOracleGrainBase<RsaSignatureResult>,
        IOracleObserverRsaSignatureCaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IShaFactory _shaFactory;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IRandom800_90 _rand;

        private RsaSignatureParameters _param;

        public OracleObserverRsaSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IShaFactory shaFactory,
            IPaddingFactory paddingFactory,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rsa = rsa;
            _shaFactory = shaFactory;
            _paddingFactory = paddingFactory;
            _messageRandomizer = messageRandomizer;
            _entropyProviderFactory = entropyProviderFactory;
            _rand = rand;
        }

        public async Task<bool> BeginWorkAsync(RsaSignatureParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var messageLength = _param.MessageLength == 0 ? _param.Modulo / 2 : _param.MessageLength;

            var message = _rand.GetRandomBitString(messageLength);
            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var salt = _rand.GetRandomBitString(_param.SaltLength * 8); // Comes in bytes, convert to bits
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(_param.PaddingScheme, sha, _param.MaskFunction, entropyProvider, _param.SaltLength);

            var messageCopy = message.GetDeepCopy();
            BitString randomValue = null;
            if (_param.IsMessageRandomized)
            {
                randomValue = _rand.GetRandomBitString(_param.HashAlg.OutputLen);
                var entropyProviderRandomizedMessage = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProviderRandomizedMessage.AddEntropy(randomValue);
                messageCopy = _messageRandomizer.WithEntropyProvider(entropyProviderRandomizedMessage).Build()
                    .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
            }

            var result = new SignatureBuilder()
                .WithDecryptionScheme(_rsa)
                .WithMessage(messageCopy)
                .WithPaddingScheme(paddingScheme)
                .WithKey(_param.Key)
                .BuildSign();

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new RsaSignatureResult
            {
                Message = message,
                RandomValue = randomValue,
                Signature = new BitString(result.Signature),
                Salt = _param.PaddingScheme == SignatureSchemes.Pss ? salt : null
            });
        }
    }
}
