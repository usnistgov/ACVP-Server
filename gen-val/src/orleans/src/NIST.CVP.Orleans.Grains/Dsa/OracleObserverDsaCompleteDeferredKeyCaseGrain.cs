using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.Orleans.Grains.Dsa
{
    public class OracleObserverDsaCompleteDeferredKeyCaseGrain : ObservableOracleGrainBase<VerifyResult<DsaKeyResult>>, 
        IOracleObserverDsaCompleteDeferredKeyCaseGrain
    {
        
        private readonly IDsaFfcFactory _dsaFactory;

        private DsaKeyParameters _param;
        private DsaKeyResult _fullParam;

        public OracleObserverDsaCompleteDeferredKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IDsaFfcFactory dsaFactory
        ) : base (nonOrleansScheduler)
        {
            _dsaFactory = dsaFactory;
        }
        
        public async Task<bool> BeginWorkAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
            var dsa = _dsaFactory.GetInstance(hashFunction);

            var result = dsa.ValidateKeyPair(_param.DomainParameters, _fullParam.Key);

            // Notify observers of result
            await Notify(new VerifyResult<DsaKeyResult>
            {
                Result = result.Success,
                VerifiedValue = _fullParam
            });
        }
    }
}