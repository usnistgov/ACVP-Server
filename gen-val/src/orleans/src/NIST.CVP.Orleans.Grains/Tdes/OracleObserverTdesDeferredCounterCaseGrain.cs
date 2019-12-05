using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Ctr;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Orleans.Grains.Tdes
{
    public class OracleObserverTdesDeferredCounterCaseGrain : ObservableOracleGrainBase<TdesResult>, 
        IOracleObserverTdesDeferredCounterCaseGrain
    {
        private readonly IEntropyProvider _entropyProvider;

        private CounterParameters<TdesParameters> _param;

        public OracleObserverTdesDeferredCounterCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(CounterParameters<TdesParameters> param)
        {
            _param = param;

            await BeginGrainWorkAsync();
            return await Task.FromResult(true);
        }
        
        protected override async Task DoWorkAsync()
        {
            await Notify(CounterHelpers.GetDeferredCounterTest(_param, _entropyProvider));
        }
    }
}