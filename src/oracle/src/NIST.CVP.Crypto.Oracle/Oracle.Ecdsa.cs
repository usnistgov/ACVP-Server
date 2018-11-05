using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public virtual async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            var poolBoy = new PoolBoy<EcdsaKeyResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.ECDSA_KEY);
            if (poolResult != null)
            {
                return poolResult;
            }

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaKeyCaseGrain, EcdsaKeyResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaCompleteDeferredKeyCaseGrain, EcdsaKeyResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            var key = await GetEcdsaKeyAsync(param);

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaVerifyKeyCaseGrain, VerifyResult<EcdsaKeyResult>>();
            await observableGrain.Grain.BeginWorkAsync(param, key);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaDeferredSignatureCaseGrain, EcdsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain, VerifyResult<EcdsaSignatureResult>>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaSignatureCaseGrain, EcdsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

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
                await _clusterClient.GetObserverGrain<IOracleObserverEcdsaVerifySignatureCaseGrain, VerifyResult<EcdsaSignatureResult>>();
            await observableGrain.Grain.BeginWorkAsync(param, key, badKey);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
