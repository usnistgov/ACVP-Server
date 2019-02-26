using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces.Aead;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCcmCaseGrain, AeadResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }
        
        public async Task<AeadResult> GetEcmaCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCcmEcmaCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesGcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesXpnCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }
        
        public async Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesDeferredGcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCompleteDeferredGcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesDeferredXpnCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAesCompleteDeferredXpnCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
