using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1
{
    public class OracleObserverKasAftFfcCaseGrain : ObservableOracleGrainBase<KasAftResultFfc>,
        IOracleObserverKasAftFfcCaseGrain
    {
        private readonly IKasAftTestGeneratorFactory<KasAftParametersFfc, KasAftResultFfc> _kasFactory;
        private readonly ILogger<OracleObserverKasAftFfcCaseGrain> _logger;

        private KasAftParametersFfc _param;


        public OracleObserverKasAftFfcCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IKasAftTestGeneratorFactory<KasAftParametersFfc, KasAftResultFfc> kasFactory,
            ILogger<OracleObserverKasAftFfcCaseGrain> logger
        ) : base(nonOrleansScheduler)
        {
            _kasFactory = kasFactory;
            _logger = logger;
        }

        public async Task<bool> BeginWorkAsync(KasAftParametersFfc param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }

        protected override async Task DoWorkAsync()
        {
            try
            {
                await Notify(_kasFactory.GetInstance(_param.KasMode).GetTest(_param));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Param: {JsonConvert.SerializeObject(_param)}");
                await Throw(e);
            }
        }
    }
}
