using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1
{
    public class OracleObserverKasAftEccCaseGrain : ObservableOracleGrainBase<KasAftResultEcc>, 
        IOracleObserverKasAftEccCaseGrain
    {
        private readonly IKasAftTestGeneratorFactory<KasAftParametersEcc, KasAftResultEcc> _kasFactory;

        private KasAftParametersEcc _param;

        public OracleObserverKasAftEccCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasAftTestGeneratorFactory<KasAftParametersEcc, KasAftResultEcc> kasFactory
        ) : base (nonOrleansScheduler)
        {
            _kasFactory = kasFactory;
        }
        
        public async Task<bool> BeginWorkAsync(KasAftParametersEcc param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            try
            {
                // Notify observers of result
                await Notify(_kasFactory.GetInstance(_param.KasMode).GetTest(_param));
            }
            catch (Exception ex)
            {
                await Throw(ex);
            }
        }
    }
}