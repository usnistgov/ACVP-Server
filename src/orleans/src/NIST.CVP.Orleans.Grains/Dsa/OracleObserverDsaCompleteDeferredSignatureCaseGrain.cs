using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.Orleans.Grains.Dsa
{
    public class OracleObserverDsaCompleteDeferredSignatureCaseGrain : ObservableOracleGrainBase<VerifyResult<DsaSignatureResult>>, 
        IOracleObserverDsaCompleteDeferredSignatureCaseGrain
    {
        
        private readonly IDsaFfcFactory _dsaFactory;

        private DsaSignatureParameters _param;
        private DsaSignatureResult _fullParam;

        public OracleObserverDsaCompleteDeferredSignatureCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFactory
        ) : base (nonOrleansScheduler)
        {
            _dsaFactory = dsaFactory;
        }
        
        public async Task<bool> BeginWorkAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var ffcDsa = _dsaFactory.GetInstance(_param.HashAlg);
            var verifyResult = ffcDsa.Verify(_param.DomainParameters, _fullParam.Key, _fullParam.Message, _fullParam.Signature);

            // Notify observers of result
            await Notify(new VerifyResult<DsaSignatureResult>
            {
                Result = verifyResult.Success,
                VerifiedValue = _fullParam
            });
        }
    }
}