using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverEcdsaCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<EcdsaSignatureResult>>, 
        IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain
    {
        private readonly IEccCurveFactory _curveFactory;
        private readonly IDsaEccFactory _dsaFactory;

        private EcdsaSignatureParameters _param;
        private EcdsaSignatureResult _fullParam;

        public OracleObserverEcdsaCompleteDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEccCurveFactory curveFactory,
            IDsaEccFactory dsaFactory
        ) : base (nonOrleansScheduler)
        {
            _curveFactory = curveFactory;
            _dsaFactory = dsaFactory;
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
            var eccDsa = _dsaFactory.GetInstance(_param.HashAlg);
            var curve = _curveFactory.GetCurve(_param.Curve);
            var domainParams = new EccDomainParameters(curve);

            var result = eccDsa.Verify(domainParams, _param.Key, _fullParam.Message, _fullParam.Signature, _param.PreHashedMessage);

            // Notify observers of result
            await Notify(new VerifyResult<EcdsaSignatureResult>
            {
                Result = result.Success
            });
        }
    }
}