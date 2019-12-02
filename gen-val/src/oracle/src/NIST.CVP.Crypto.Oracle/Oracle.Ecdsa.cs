using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public virtual async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaKeyCaseGrain, EcdsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaCompleteDeferredKeyCaseGrain, EcdsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            var key = await GetEcdsaKeyAsync(param);

            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaVerifyKeyCaseGrain, VerifyResult<EcdsaKeyResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaDeferredSignatureCaseGrain, EcdsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain, VerifyResult<EcdsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaSignatureCaseGrain, EcdsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param)
        {
            var keyParam = new EcdsaKeyParameters
            {
                Curve = param.Curve
            };

            var key = await GetEcdsaKeyAsync(keyParam);
            // re-signs with "bad key" under specific error condition to ensure IUT validates as failed verification.
            var badKey = await GetEcdsaKeyAsync(keyParam);
            
            var observableGrain = 
                await GetObserverGrain<IOracleObserverEcdsaVerifySignatureCaseGrain, VerifyResult<EcdsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, badKey, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
