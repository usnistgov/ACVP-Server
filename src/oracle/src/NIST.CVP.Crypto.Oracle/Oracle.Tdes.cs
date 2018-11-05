using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesCaseGrain, TdesResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TdesResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TDES_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesMctCaseGrain, MctResult<TdesResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();

        }

        public async Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesDeferredCounterCaseGrain, TdesResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesCompleteDeferredCounterCaseGrain, TdesResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTdesCounterExtractIvsCaseGrain, CounterResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
