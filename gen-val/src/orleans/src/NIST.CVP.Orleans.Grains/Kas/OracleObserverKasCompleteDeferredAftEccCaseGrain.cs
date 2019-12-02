using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;
using NIST.CVP.Orleans.Grains.Kas.Ecc;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public class OracleObserverKasCompleteDeferredAftEccCaseGrain : ObservableOracleGrainBase<KasAftDeferredResult>, 
        IOracleObserverKasCompleteDeferredAftEccCaseGrain
    {
        private readonly IKasAftDeferredTestResolverFactory<KasAftDeferredParametersEcc, KasAftDeferredResult> _kasFactory;

        private KasAftDeferredParametersEcc _param;

        public OracleObserverKasCompleteDeferredAftEccCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasAftDeferredTestResolverFactory<KasAftDeferredParametersEcc, KasAftDeferredResult> kasFactory
        ) : base (nonOrleansScheduler)
        {
            _kasFactory = kasFactory;
        }
        
        public async Task<bool> BeginWorkAsync(KasAftDeferredParametersEcc param)
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