using System;
using System.Threading.Tasks;
using NIST.CVP.Common;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Ctr;
using NIST.CVP.Orleans.Grains.Interfaces;

namespace NIST.CVP.Orleans.Grains
{
    public class OracleObserverAesDeferredCounterCaseGrain : ObservableOracleGrainBase<AesResult>, 
        IOracleObserverAesDeferredCounterCaseGrain
    {
        private readonly IEntropyProvider _entropyProvider;

        private CounterParameters<AesParameters> _param;

        public OracleObserverAesDeferredCounterCaseGrain(
            LimitedConcurrencyLevelTaskScheduler nonOrleansScheduler,
            IEntropyProviderFactory entropyProviderFactory
        ) : base (nonOrleansScheduler)
        {
            _entropyProvider = entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public async Task<bool> BeginWorkAsync(CounterParameters<AesParameters> param)
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