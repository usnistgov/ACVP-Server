using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.RSA.Signatures;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
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
            IPaddingScheme paddingScheme;
            
            // shouldn't need to check if the scheme is PSS... if the Mode is SHAKE, then the scheme should be PSS.
            if (_param.PaddingScheme == SignatureSchemes.Pss && _param.HashAlg.Mode == ModeValues.SHAKE)
            {
                // Since we're using an XOF, we need to specify the outputLen. Otherwise the default outputLens from
                // ShaAttributes would be used and FIPS 186-5 requires different values to be used, i.e., 256 and 512
                paddingScheme = _paddingFactory.GetPaddingScheme(_param.PaddingScheme, sha, _param.MaskFunction, entropyProvider, _param.SaltLength, _param.HashAlg.OutputLen);   
            }
            else
            {
                paddingScheme = _paddingFactory.GetPaddingScheme(_param.PaddingScheme, sha, _param.MaskFunction, entropyProvider, _param.SaltLength);
            }

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
