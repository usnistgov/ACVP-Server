using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTdesCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<TdesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<TdesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            var poolBoy = new PoolBoy<MctResult<TdesResult>>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.TDES_MCT);
            if (poolResult != null)
            {
                return poolResult;
            }

            var grain = _clusterClient.GetGrain<IOracleObserverTdesMctCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MctResult<TdesResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MctResult<TdesResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTdesDeferredCounterCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<TdesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<TdesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTdesCompleteDeferredCounterCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<TdesResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<TdesResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTdesCounterExtractIvsCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<CounterResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<CounterResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
