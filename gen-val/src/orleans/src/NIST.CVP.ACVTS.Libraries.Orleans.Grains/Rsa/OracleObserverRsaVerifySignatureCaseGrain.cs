using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
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
    public class OracleObserverRsaVerifySignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<RsaSignatureResult>>,
        IOracleObserverRsaVerifySignatureCaseGrain
    {
        private readonly IRsa _rsa;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private readonly IRandom800_90 _rand;

        private RsaSignatureParameters _param;

        public OracleObserverRsaVerifySignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsa rsa,
            IPaddingFactory paddingFactory,
            IShaFactory shaFactory,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory,
            IRandom800_90 rand
        ) : base(nonOrleansScheduler)
        {
            _rsa = rsa;
            _paddingFactory = paddingFactory;
            _shaFactory = shaFactory;
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
            var message = _rand.GetRandomBitString(_param.Modulo / 2);
            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var salt = _rand.GetRandomBitString(_param.SaltLength * 8); // Comes in bytes, convert to bits
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(salt);

            var paddingScheme = _paddingFactory.GetSigningPaddingScheme(_param.PaddingScheme, sha, _param.Reason, _param.MaskFunction, entropyProvider, _param.SaltLength);

            var copyKey = new KeyPair
            {
                PrivKey = _param.Key.PrivKey,
                PubKey = new PublicKey
                {
                    E = _param.Key.PubKey.E,
                    N = _param.Key.PubKey.N
                }
            };

            var messageCopy = message.GetDeepCopy();
            BitString randomValue = null;
            if (_param.IsMessageRandomized)
            {
                randomValue = _rand.GetRandomBitString(_param.HashAlg.OutputLen);
                var entropyProviderRandomMessage = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProviderRandomMessage.AddEntropy(randomValue);
                messageCopy = _messageRandomizer.WithEntropyProvider(entropyProviderRandomMessage).Build()
                    .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
            }

            var result = new SignatureBuilder()
                .WithDecryptionScheme(_rsa)
                .WithMessage(messageCopy)
                .WithPaddingScheme(paddingScheme)
                .WithKey(copyKey)
                .BuildSign();

            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new VerifyResult<RsaSignatureResult>
            {
                Result = _param.Reason == SignatureModifications.None,
                VerifiedValue = new RsaSignatureResult
                {
                    Key = copyKey,
                    Message = message,
                    RandomValue = randomValue,
                    Signature = new BitString(result.Signature),
                    Salt = _param.PaddingScheme == SignatureSchemes.Pss ? salt : null
                }
            });
        }
    }
}
