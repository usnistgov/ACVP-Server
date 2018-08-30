using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Orleans.Grains.Ecdsa
{
    public class OracleObserverEcdsaKeyCaseCaseGrain : ObservableOracleGrainBase<EcdsaKeyResult>, 
        IOracleObserverEcdsaKeyCaseGrain
    {
        private readonly IEcdsaKeyGenRunner _runner;

        private EcdsaKeyParameters _param;

        public OracleObserverEcdsaKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEcdsaKeyGenRunner runner
        ) : base (nonOrleansScheduler)
        {
            _runner = runner;
        }
        
        public async Task<bool> BeginWorkAsync(EcdsaKeyParameters param)
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