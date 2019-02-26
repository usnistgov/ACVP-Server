using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Kas;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KasValResultEcc> GetKasValTestEccAsync(KasValParametersEcc param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasValEccCaseGrain, KasValResultEcc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftResultEcc> GetKasAftTestEccAsync(KasAftParametersEcc param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasAftEccCaseGrain, KasAftResultEcc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersEcc param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasCompleteDeferredAftEccCaseGrain, KasAftDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasValResultFfc> GetKasValTestFfcAsync(KasValParametersFfc param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasValFfcCaseGrain, KasValResultFfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftResultFfc> GetKasAftTestFfcAsync(KasAftParametersFfc param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasAftFfcCaseGrain, KasAftResultFfc>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasAftDeferredResult> CompleteDeferredKasTestAsync(KasAftDeferredParametersFfc param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasCompleteDeferredAftFfcCaseGrain, KasAftDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasEccComponentResult> GetKasEccComponentTestAsync(KasEccComponentParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasEccComponentCaseGrain, KasEccComponentResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KasEccComponentDeferredResult> CompleteDeferredKasComponentTestAsync(KasEccComponentDeferredParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKasEccComponentCompleteDeferredCaseGrain, KasEccComponentDeferredResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
