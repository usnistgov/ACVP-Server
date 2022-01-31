using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Rsa
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
        ) : base(nonOrleansScheduler)
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
