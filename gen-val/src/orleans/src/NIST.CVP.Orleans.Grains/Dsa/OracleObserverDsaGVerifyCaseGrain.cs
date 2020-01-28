using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.Orleans.Grains.Dsa
{
    public class OracleObserverDsaGVerifyCaseGrain : ObservableOracleGrainBase<VerifyResult<DsaDomainParametersResult>>, 
        IOracleObserverDsaGVerifyCaseGrain
    {

        private readonly IShaFactory _shaFactory;
        private readonly IGGeneratorValidatorFactory _gGenFactory;

        private DsaDomainParametersParameters _param;
        private DsaDomainParametersResult _fullParam;

        public OracleObserverDsaGVerifyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IShaFactory shaFactory,
            IGGeneratorValidatorFactory gGenFactory
        ) : base (nonOrleansScheduler)
        {
            _shaFactory = shaFactory;
            _gGenFactory = gGenFactory;
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
            var gGen = _gGenFactory.GetGeneratorValidator(_param.GGenMode, sha);

            var result = gGen.Validate(_fullParam.P, _fullParam.Q, _fullParam.G, _fullParam.Seed, _fullParam.Index);

            // Notify observers of result
            await Notify(new VerifyResult<DsaDomainParametersResult>
            {
                Result = result.Success,
                VerifiedValue = _fullParam
            });
        }
    }
}