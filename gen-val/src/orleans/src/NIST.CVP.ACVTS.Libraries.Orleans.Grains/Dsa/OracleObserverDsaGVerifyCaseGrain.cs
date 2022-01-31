using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Dsa
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
        ) : base(nonOrleansScheduler)
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
