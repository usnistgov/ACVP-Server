using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        public virtual async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            IRandom800_90 rand = new Random800_90();
            IKeyGenParameterHelper keyGenHelper = new KeyGenParameterHelper(rand);

            RsaPrimeResult result = null;
            
            while (true)
            {
                param.Seed = keyGenHelper.GetSeed(param.Modulus);
                param.PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? 
                    param.PublicExponent : 
                    keyGenHelper.GetEValue(RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX);
                param.BitLens = keyGenHelper.GetBitlens(param.Modulus, param.KeyMode);
                
                // Generate key until success
                result = await GetRsaPrimes(param);

                if (result.Success)
                {
                    break;
                }
            }

            return new RsaKeyResult
            {
                Key = result.Key,
                AuxValues = result.Aux,
                BitLens = param.BitLens,
                Seed = param.Seed
            };
        }

        public async Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaCompleteKeyCaseGrain, RsaKeyResult>();
            await observableGrain.Grain.BeginWorkAsync(param, keyMode);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaCompleteDeferredKeyCaseGrain, RsaKeyResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaVerifyKeyCaseGrain, VerifyResult<RsaKeyResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = param.KeyFormat,
                Modulus = param.Modulo,
                PrimeTest = PrimeTestModes.C2,
                PublicExponentMode = PublicExponentModes.Random,
                KeyMode = PrimeGenModes.B33
            };

            var key = await GetRsaKeyAsync(keyParam);

            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaSignaturePrimitiveCaseGrain, RsaSignaturePrimitiveResult>();
            await observableGrain.Grain.BeginWorkAsync(param, key);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaDeferredSignatureCaseGrain, RsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaCompleteDeferredSignatureCaseGrain, VerifyResult<RsaSignatureResult>>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaSignatureCaseGrain, RsaSignatureResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaVerifySignatureCaseGrain, VerifyResult<RsaSignatureResult>>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaDeferredDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param,
            RsaDecryptionPrimitiveResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<RsaPrimeResult> GetRsaPrimes(RsaKeyParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverRsaGeneratePrimesCaseGrain, RsaPrimeResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

