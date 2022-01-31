using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Dsa
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
        ) : base(nonOrleansScheduler)
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
