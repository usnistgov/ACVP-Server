using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Dsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Dsa
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
        ) : base(nonOrleansScheduler)
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
