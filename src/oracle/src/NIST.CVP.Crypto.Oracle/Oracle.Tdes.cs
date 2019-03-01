using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTdesCaseGrain, TdesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTdesMctCaseGrain, MctResult<TdesResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTdesDeferredCounterCaseGrain, TdesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTdesCompleteDeferredCounterCaseGrain, TdesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTdesCounterExtractIvsCaseGrain, CounterResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
