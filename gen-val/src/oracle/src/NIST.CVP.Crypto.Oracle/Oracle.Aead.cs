using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces.Aead;
using NIST.CVP.Crypto.Oracle.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
        
        public async Task<AeadResult> GetEcmaCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCcmEcmaCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesGcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetAesGcmSivCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesGcmSivCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesXpnCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
        
        public async Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesDeferredGcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCompleteDeferredGcmCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesDeferredXpnCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAesCompleteDeferredXpnCaseGrain, AeadResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
