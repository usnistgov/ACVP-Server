using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverDsaPqVerifyCaseCaseGrain : ObservableOracleGrainBase<VerifyResult<DsaDomainParametersResult>>, 
        IOracleObserverDsaPqVerifyCaseGrain
    {

        private readonly IShaFactory _shaFactory;
        private readonly IPQGeneratorValidatorFactory _pqGenFactory;

        private DsaDomainParametersParameters _param;
        private DsaDomainParametersResult _fullParam;

        public OracleObserverDsaPqVerifyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IPQGeneratorValidatorFactory pqGenFactory
        ) : base (nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _pqGenFactory = pqGenFactory;
        }
        
        public async Task<bool> BeginWorkAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            _param = param;
            _fullParam = fullParam;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var sha = _shaFactory.GetShaInstance(_param.HashAlg);
            var pqGen = _pqGenFactory.GetGeneratorValidator(_param.PQGenMode, sha, EntropyProviderTypes.Random);

            var result = pqGen.Validate(_fullParam.P, _fullParam.Q, _fullParam.Seed, _fullParam.Counter);

            // Notify observers of result
            await Notify(new VerifyResult<DsaDomainParametersResult>
            {
                Result = result.Success,
                VerifiedValue = _fullParam
            });
        }
    }
}