using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasValEccCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasValResultEcc>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasValResultEcc>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasAftEccCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasAftResultEcc>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasAftResultEcc>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasCompleteDeferredAftEccCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasAftDeferredResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasAftDeferredResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasValFfcCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasValResultFfc>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasValResultFfc>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasAftFfcCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasAftResultFfc>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasAftResultFfc>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasCompleteDeferredAftFfcCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasAftDeferredResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasAftDeferredResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasEccComponentCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasEccComponentResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasEccComponentResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKasEccComponentCompleteDeferredCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KasEccComponentDeferredResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KasEccComponentDeferredResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
