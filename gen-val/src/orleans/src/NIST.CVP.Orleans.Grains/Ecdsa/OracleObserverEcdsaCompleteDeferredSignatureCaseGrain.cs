using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Ecdsa
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