using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<EcdsaSignatureResult>>,
        IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;
        private readonly IPreSigVerMessageRandomizerBuilder _messageRandomizer;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        private EcdsaSignatureParameters _param;
        private EcdsaSignatureResult _fullParam;

        public OracleObserverEcdsaCompleteDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory,
            IPreSigVerMessageRandomizerBuilder messageRandomizer,
            IEntropyProviderFactory entropyProviderFactory
        ) : base(nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _messageRandomizer = messageRandomizer;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public async Task<bool> BeginWorkAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            var messageCopy = _fullParam.Message.GetDeepCopy();

            if (_param.IsMessageRandomized)
            {
                var entropyProvider = _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(_fullParam.RandomValue);
                messageCopy = _messageRandomizer.WithEntropyProvider(entropyProvider).Build()
                    .RandomizeMessage(messageCopy, _param.HashAlg.OutputLen);
            }

            var eccDsa = _dsaFactory.GetInstanceForVerification(_param.HashAlg);
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EccDomainParameters(curve);

            var result = eccDsa.Verify(domainParams, _param.Key, messageCopy, _fullParam.Signature, _param.PreHashedMessage);

            // Notify observers of result
            await Notify(new VerifyResult<EcdsaSignatureResult>
            {
                Result = result.Success
            });
        }
    }
}
