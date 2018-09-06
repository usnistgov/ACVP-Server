using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaKeyCaseGrain : ObservableOracleGrainBase<RsaKeyResult>, 
        IOracleObserverRsaKeyCaseGrain
    {
        private readonly IRsaRunner _rsaRunner;
        
        private RsaKeyParameters _param;

        public OracleObserverRsaKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsaRunner rsaRunner
        ) : base (nonOrleansScheduler)
        {
            _rsaRunner = rsaRunner;
        }
        
        public async Task<bool> BeginWorkAsync(RsaKeyParameters param)
        {
            _param = param;
            
            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            var result = _rsaRunner.GetRsaKey(_param);

            // Notify observers of result
            await Notify(result);
        }
    }
}