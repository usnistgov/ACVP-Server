using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaKeyCaseCaseGrain : ObservableOracleGrainBase<EddsaKeyResult>,
        IOracleObserverEddsaKeyCaseGrain
    {
        private readonly IEddsaKeyGenRunner _runner;

        private EddsaKeyParameters _param;

        public OracleObserverEddsaKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEddsaKeyGenRunner runner
        ) : base(nonOrleansScheduler)
        {
            _runner = runner;
        }

        public async Task<bool> BeginWorkAsync(EddsaKeyParameters param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(_runner.GenerateKey(_param));
        }
    }
}
