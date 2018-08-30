using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverEddsaCompleteDeferredKeyCaseGrain : ObservableOracleGrainBase<EddsaKeyResult>, 
        IOracleObserverEddsaCompleteDeferredKeyCaseGrain
    {
        private readonly IEdwardsCurveFactory _curveFactory;
        private readonly IDsaEdFactory _dsaFactory;
        private readonly IShaFactory _shaFactory;

        private EddsaKeyParameters _param;
        private EddsaKeyResult _fullParam;

        public OracleObserverEddsaCompleteDeferredKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEdwardsCurveFactory curveFactory,
            IDsaEdFactory dsaFactory,
            IShaFactory shaFactory
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
            _shaFactory = shaFactory;
        }
        
        public async Task<bool> BeginWorkAsync(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EdDomainParameters(curve, _shaFactory);

            var edDsa = _dsaFactory.GetInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256), EntropyProviderTypes.Testable);
            edDsa.AddEntropy(_fullParam.Key.PrivateD.ToPositiveBigInteger());

            var result = edDsa.GenerateKeyPair(domainParams);
            if (!result.Success)
            {
                throw new Exception();
            }

            // Notify observers of result
            await Notify(new EddsaKeyResult
            {
                Key = result.KeyPair
            });
        }
    }
}