using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Orleans.Grains.Rsa
{
    public class OracleObserverRsaCompleteKeyCaseGrain : ObservableOracleGrainBase<RsaKeyResult>, 
        IOracleObserverRsaCompleteKeyCaseGrain
    {
        private readonly IRsaRunner _rsaRunner;
        
        private RsaKeyResult _param;
        private PrivateKeyModes _keyMode;

        public OracleObserverRsaCompleteKeyCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IRsaRunner rsaRunner
        ) : base (nonOrleansScheduler)
        {
            _rsaRunner = rsaRunner;
        }
        
        public async Task<bool> BeginWorkAsync(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            _param = param;
            _keyMode = keyMode;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(_rsaRunner.CompleteKey(_param, _keyMode));
        }
    }
}