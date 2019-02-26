using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesCaseGrain, TdesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesMctCaseGrain, MctResult<TdesResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesDeferredCounterCaseGrain, TdesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesCompleteDeferredCounterCaseGrain, TdesResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesCounterExtractIvsCaseGrain, CounterResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
