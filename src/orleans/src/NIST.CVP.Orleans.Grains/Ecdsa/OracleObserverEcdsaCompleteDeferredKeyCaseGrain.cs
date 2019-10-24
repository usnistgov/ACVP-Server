using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaCompleteDeferredKeyCaseGrain : ObservableOracleGrainBase<EcdsaKeyResult>, 
        IOracleObserverEcdsaCompleteDeferredKeyCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;

        private EcdsaKeyParameters _param;
        private EcdsaKeyResult _fullParam;

        public OracleObserverEcdsaCompleteDeferredKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EccDomainParameters(curve);

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(_fullParam.Key.PrivateD);

            var eccDsa = _dsaFactory.GetInstanceForKeys(entropyProvider);

            var result = eccDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new EcdsaKeyResult
            {
                Key = result.KeyPair
            });
        }
    }
}