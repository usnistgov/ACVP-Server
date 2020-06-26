using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1
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
            try
            {
                await Notify(_kasFactory.GetInstance(_param.KasMode).CompleteTest(_param));
            }
            catch (Exception e)
            {
                await Throw(e);
            }
        }
    }
}