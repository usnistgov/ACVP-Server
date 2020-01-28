using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaKeyCaseGrain, EddsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaCompleteDeferredKeyCaseGrain, EddsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaVerifyKeyCaseGrain, VerifyResult<EddsaKeyResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaDeferredSignatureCaseGrain, EddsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaCompleteDeferredSignatureCaseGrain, VerifyResult<EddsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaSignatureCaseGrain, EddsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaVerifySignatureCaseGrain, VerifyResult<EddsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        // These can be merged or removed depending on test group bit flip
        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaDeferredSignatureBitFlipCaseGrain, EddsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaSignatureBitFlipCaseGrain, EddsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        // Remove or do something... this is a little awkward how it is done
        public async Task<BitString> GetEddsaMessageBitFlipAsync(EddsaMessageParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEddsaMessageBitFlipCaseGrain, BitString>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
