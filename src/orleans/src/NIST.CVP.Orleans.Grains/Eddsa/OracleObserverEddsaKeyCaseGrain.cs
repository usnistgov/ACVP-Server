using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.Orleans.Grains.Eddsa
{
    public class OracleObserverEddsaKeyCaseCaseGrain : ObservableOracleGrainBase<EddsaKeyResult>, 
        IOracleObserverEddsaKeyCaseGrain
    {
        private readonly IEddsaKeyGenRunner _runner;

        private EddsaKeyParameters _param;

        public OracleObserverEddsaKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEddsaKeyGenRunner runner
        ) : base (nonOrleansScheduler)
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