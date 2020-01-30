using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaKeyCaseGrain, EddsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetEddsaKeyAsync(param);
            }
        }

        public async Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaCompleteDeferredKeyCaseGrain, EddsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await CompleteDeferredEddsaKeyAsync(param, fullParam);
            }
        }

        public async Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaVerifyKeyCaseGrain, VerifyResult<EddsaKeyResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetEddsaKeyVerifyAsync(param);
            }
        }

        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaDeferredSignatureCaseGrain, EddsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetDeferredEddsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaCompleteDeferredSignatureCaseGrain, VerifyResult<EddsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await CompleteDeferredEddsaSignatureAsync(param, fullParam);
            }
        }

        public async Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaSignatureCaseGrain, EddsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetEddsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaVerifySignatureCaseGrain, VerifyResult<EddsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetEddsaVerifyResultAsync(param);
            }
        }

        // These can be merged or removed depending on test group bit flip
        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaDeferredSignatureBitFlipCaseGrain, EddsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetDeferredEddsaSignatureBitFlipAsync(param);
            }
        }

        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaSignatureBitFlipCaseGrain, EddsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetEddsaSignatureBitFlipAsync(param);
            }
        }

        // Remove or do something... this is a little awkward how it is done
        public async Task<BitString> GetEddsaMessageBitFlipAsync(EddsaMessageParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverEddsaMessageBitFlipCaseGrain, BitString>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, JsonConvert.SerializeObject(param));
                return await GetEddsaMessageBitFlipAsync(param);
            }
        }
    }
}
