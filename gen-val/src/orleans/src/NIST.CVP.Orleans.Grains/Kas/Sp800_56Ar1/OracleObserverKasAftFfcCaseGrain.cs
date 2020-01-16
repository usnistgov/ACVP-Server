using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1
{
    public class OracleObserverKasAftFfcCaseGrain : ObservableOracleGrainBase<KasAftResultFfc>, 
        IOracleObserverKasAftFfcCaseGrain
    {
        private readonly IKasAftTestGeneratorFactory<KasAftParametersFfc, KasAftResultFfc> _kasFactory;

        private KasAftParametersFfc _param;

        public OracleObserverKasAftFfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasAftTestGeneratorFactory<KasAftParametersFfc, KasAftResultFfc> kasFactory
        ) : base (nonOrleansScheduler)
        {
            _kasFactory = kasFactory;
        }
        
        public async Task<bool> BeginWorkAsync(KasAftParametersFfc param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            // Notify observers of result
            await Notify(_kasFactory.GetInstance(_param.KasMode).GetTest(_param));
        }
    }
}