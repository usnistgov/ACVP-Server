﻿using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;
using System.Threading.Tasks;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<RsaSignatureResult>>,
        IOracleObserverRsaCompleteDeferredSignatureCaseGrain
    {
        private readonly IShaFactory _shaFactory;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IRsa _rsa;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        private RsaSignatureParameters _param;
        private RsaSignatureResult _fullParam;

        public OracleObserverRsaCompleteDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IPaddingFactory paddingFactory,
            IRsa rsa,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _paddingFactory = paddingFactory;
            _rsa = rsa;
            _messageRandomizer = messageRandomizer;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public async Task<bool> BeginWorkAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(_fullParam.Salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(_param.PaddingScheme, sha, entropyProvider, _param.SaltLength);

            var messageCopy = _fullParam.Message.GetDeepCopy();

            if (_param.IsMessageRandomized)
            {
                var entropyProviderRandomizedMessage = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProviderRandomizedMessage.AddEntropy(_fullParam.RandomValue);
                messageCopy = _messageRandomizer.WithEntropyProvider(entropyProviderRandomizedMessage).Build()
                    .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
            }

            var result = new SignatureBuilder()
                .WithDecryptionScheme(_rsa)
                .WithKey(_param.Key)
                .WithMessage(messageCopy)
                .WithPaddingScheme(paddingScheme)
                .WithSignature(_fullParam.Signature)
                .BuildVerify();

            // Notify observers of result
            await Notify(new VerifyResult<RsaSignatureResult>
            {
                VerifiedValue = _fullParam,
                Result = result.Success
            });
        }
    }
}