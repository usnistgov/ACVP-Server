using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Pools.Services;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        public virtual async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            RsaPrimeResult result;
            do
            {
                param.Seed = KeyGenHelper.GetSeed(param.Modulus);
                param.PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed ? 
                    param.PublicExponent : 
                    KeyGenHelper.GetEValue(param.Standard, RSA_PUBLIC_EXPONENT_BITS_MIN, RSA_PUBLIC_EXPONENT_BITS_MAX);
                param.BitLens = KeyGenHelper.GetBitlens(param.Modulus, param.KeyMode);
                
                // Generate key until success
                result = await GetRsaPrimes(param);

                // TODO This is debug code due to RSA keygen getting into an infinite loop,
                // this should be hit very seldom except in cases where we hit the error condition we're trying to monitor for.
                if (!result.Success || result.Key?.PrivKey?.P == 0 || result.Key?.PrivKey?.Q == 0)
                {
                    var serializerSettings = new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        Converters = new JsonConverterProvider().GetJsonConverters()
                    };
                   
                    var jsonParam = JsonConvert.SerializeObject(param, serializerSettings);
                    ThisLogger.Warn($"KeyGen failed with the following parameter: {jsonParam}");

                    var jsonResult = JsonConvert.SerializeObject(result, serializerSettings);
                    ThisLogger.Warn($"KeyGen result: {jsonResult}");
                }
                
            } while (!result.Success);
            
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
                await GetObserverGrain<IOracleObserverRsaCompleteKeyCaseGrain, RsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, keyMode, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaCompleteDeferredKeyCaseGrain, RsaKeyResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaVerifyKeyCaseGrain, VerifyResult<RsaKeyResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = param.KeyFormat,
                Modulus = param.Modulo,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                PublicExponentMode = PublicExponentModes.Random,
                KeyMode = PrimeGenModes.RandomProbablePrimes,
                Standard = Fips186Standard.Fips186_4
            };

            var key = await GetRsaKeyAsync(keyParam);

            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaSignaturePrimitiveCaseGrain, RsaSignaturePrimitiveResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaDeferredSignatureCaseGrain, RsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaCompleteDeferredSignatureCaseGrain, VerifyResult<RsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaSignatureCaseGrain, RsaSignatureResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaVerifySignatureCaseGrain, VerifyResult<RsaSignatureResult>>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaDeferredDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param,
            RsaDecryptionPrimitiveResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            KeyResult key = null;
            if (param.TestPassed)
            {
                var keyParam = new RsaKeyParameters()
                {
                    KeyMode = PrimeGenModes.RandomProbablePrimes,
                    Modulus = param.Modulo,
                    PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                    KeyFormat = PrivateKeyModes.Standard,
                    PublicExponentMode = PublicExponentModes.Random,
                    Standard = Fips186Standard.Fips186_4
                };
                var keyResult = await GetRsaKeyAsync(keyParam);
                key = new KeyResult(keyResult.Key, keyResult.AuxValues);
            }

            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public virtual async Task<RsaPrimeResult> GetRsaPrimes(RsaKeyParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverRsaGeneratePrimesCaseGrain, RsaPrimeResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}

