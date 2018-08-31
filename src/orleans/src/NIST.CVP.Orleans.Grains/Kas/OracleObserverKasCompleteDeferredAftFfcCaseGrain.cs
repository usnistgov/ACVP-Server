using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public class OracleObserverKasCompleteDeferredAftFfcCaseGrain : ObservableOracleGrainBase<KasAftDeferredResult>, 
        IOracleObserverKasCompleteDeferredAftFfcCaseGrain
    {
        private readonly IKasAftDeferredTestResolverFactory<KasAftDeferredParametersFfc, KasAftDeferredResult> _kasFactory;

        private KasAftDeferredParametersFfc _param;

        public OracleObserverKasCompleteDeferredAftFfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasAftDeferredTestResolverFactory<KasAftDeferredParametersFfc, KasAftDeferredResult> kasFactory
        ) : base (nonOrleansScheduler)
        {
            _kasFactory = kasFactory;
        }
        
        public async Task<bool> BeginWorkAsync(KasAftDeferredParametersFfc param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(_kasFactory.GetInstance(_param.KasMode).CompleteTest(_param));
        }
    }
}