using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Pools.Services;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        public virtual async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            RsaPrimeResult result = null;
            do
            {
                param.Seed = KeyGenHelper.GetSeed(param.Modulus);
                param.PublicExponent = param.PublicExponentMode == PublicExponentModes.Fixed
                    ? param.PublicExponent
                    : KeyGenHelper.GetEValue(param.Standard, RSA_PUBLIC_EXPONENT_BITS_MIN,
                        RSA_PUBLIC_EXPONENT_BITS_MAX);
                param.BitLens = KeyGenHelper.GetBitlens(param.Modulus, param.KeyMode);

                // Generate key until success
                try
                {
                    result = await GetRsaPrimes(param);
                }
                catch (Exception e)
                {
                    _logger.Warn(e, $"Failure on RSA key gen: {e.StackTrace}");
                    _logger.Warn(e, $"Failure on RSA key gen message: {e.Message}");
                    if (result == null)
                    {
                        result = new RsaPrimeResult()
                        {
                            Success = false
                        };
                    }
                    else
                    {
                        result.Success = false;
                    }
                }
                
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
                    _logger.Warn($"KeyGen failed with the following parameter: {jsonParam}");

                    var jsonResult = JsonConvert.SerializeObject(result, serializerSettings);
                    _logger.Warn($"KeyGen result: {jsonResult}");
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
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaCompleteKeyCaseGrain, RsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, keyMode, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteKeyAsync(param, keyMode);
            }
        }

        public async Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaCompleteDeferredKeyCaseGrain, RsaKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredRsaKeyCaseAsync(param, fullParam);
            }
        }

        public async Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaVerifyKeyCaseGrain, VerifyResult<RsaKeyResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaKeyVerifyAsync(param);
            }
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

            return await GetRsaSignaturePrimitiveAsync(param, key);
        }

        private async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(
            RsaSignaturePrimitiveParameters param, RsaKeyResult key)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaSignaturePrimitiveCaseGrain, RsaSignaturePrimitiveResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaSignaturePrimitiveAsync(param);
            }
        }

        public async Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaDeferredSignatureCaseGrain, RsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredRsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaCompleteDeferredSignatureCaseGrain, VerifyResult<RsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredRsaSignatureAsync(param, fullParam);
            }
        }

        public async Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaSignatureCaseGrain, RsaSignatureResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaSignatureAsync(param);
            }
        }

        public async Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaVerifySignatureCaseGrain, VerifyResult<RsaSignatureResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaVerifyAsync(param);
            }
        }

        public async Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaDeferredDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredRsaDecryptionPrimitiveAsync(param);
            }
        }

        public async Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param,
            RsaDecryptionPrimitiveResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredRsaDecryptionPrimitiveAsync(param, fullParam);
            }
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

            return await GetRsaDecryptionPrimitiveAsync(param, key);
        }

        private async Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(
            RsaDecryptionPrimitiveParameters param, KeyResult key)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaDecryptionPrimitiveCaseGrain, RsaDecryptionPrimitiveResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaDecryptionPrimitiveAsync(param, key);
            }
        }

        public virtual async Task<RsaPrimeResult> GetRsaPrimes(RsaKeyParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverRsaGeneratePrimesCaseGrain, RsaPrimeResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaPrimes(param);
            }
        }
    }
}

