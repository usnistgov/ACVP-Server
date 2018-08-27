using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesCcmCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesGcmCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesXpnCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesDeferredGcmCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesCompleteDeferredGcmCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesDeferredXpnCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAesCompleteDeferredXpnCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AeadResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AeadResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
