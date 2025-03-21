using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Xecdh
{
    public class OracleObserverXecdhKeyCaseCaseGrain : ObservableOracleGrainBase<XecdhKeyResult>,
        IOracleObserverXecdhKeyCaseGrain
    {
        private readonly IXecdhKeyGenRunner _runner;

        private XecdhKeyParameters _param;

        public OracleObserverXecdhKeyCaseCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IXecdhKeyGenRunner runner
        ) : base(nonOrleansScheduler)
        {
            _runner = runner;
        }

        public async Task<bool> BeginWorkAsync(XecdhKeyParameters param)
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
