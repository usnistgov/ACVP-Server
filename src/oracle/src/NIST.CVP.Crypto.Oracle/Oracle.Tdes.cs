using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Crypto.Symmetric.MonteCarlo;
using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Symmetric.CTR;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Enums;
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
