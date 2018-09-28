using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaKeyCaseGrain, EddsaKeyResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaCompleteDeferredKeyCaseGrain, EddsaKeyResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaVerifyKeyCaseGrain, VerifyResult<EddsaKeyResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaDeferredSignatureCaseGrain, EddsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaCompleteDeferredSignatureCaseGrain, VerifyResult<EddsaSignatureResult>>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaSignatureCaseGrain, EddsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaVerifySignatureCaseGrain, VerifyResult<EddsaSignatureResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        // These can be merged or removed depending on test group bit flip
        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaDeferredSignatureBitFlipCaseGrain, EddsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaSignatureBitFlipCaseGrain, EddsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        // Remove or do something... this is a little awkward how it is done
        public async Task<BitString> GetEddsaMessageBitFlipAsync(EddsaMessageParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEddsaMessageBitFlipCaseGrain, BitString>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
