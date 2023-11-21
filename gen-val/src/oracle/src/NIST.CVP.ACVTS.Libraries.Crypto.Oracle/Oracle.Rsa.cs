using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.JsonConverters;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        public virtual async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            // The "param" object from the pools is a shared instance from the pool manager.
            // When the pool requests orleans to fill the RSA key pool, it passes that single instance,
            // for potentially 20 values at a time.  Random is leaned on for populating *the shared
            // parameter instance*, which means all 20 instances were getting populated from a "single" seed.
            var copyParam = new RsaKeyParameters()
            {
                Modulus = param.Modulus,
                Seed = param.Seed?.GetDeepCopy(),
                Standard = param.Standard,
                BitLens = param.BitLens,
                HashAlg = param.HashAlg == null ? null : new HashFunction(param.HashAlg.Mode, param.HashAlg.DigestSize),
                KeyFormat = param.KeyFormat,
                KeyMode = param.KeyMode,
                PMod8 = param.PMod8,
                PrimeTest = param.PrimeTest,
                PublicExponent = param.PublicExponent?.GetDeepCopy(),
                QMod8 = param.QMod8,
                PublicExponentMode = param.PublicExponentMode
            };

            RsaPrimeResult result = null;
            do
            {
                copyParam.Seed = KeyGenHelper.GetSeed(_random, copyParam.Modulus);
                copyParam.PublicExponent = copyParam.PublicExponentMode == PublicExponentModes.Fixed
                    ? copyParam.PublicExponent
                    : KeyGenHelper.GetEValue(_random, copyParam.Standard, RSA_PUBLIC_EXPONENT_BITS_MIN,
                        RSA_PUBLIC_EXPONENT_BITS_MAX);
                copyParam.BitLens = KeyGenHelper.GetBitlens(_random, copyParam.Modulus, copyParam.KeyMode);

                // Generate key until success
                try
                {
                    result = await GetRsaPrimes(copyParam);
                }
                catch (RsaPrimeGenException e)
                {
                    _logger.Warn(e, $"Failure on RSA key gen guard");
                    throw;
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

                // this should be hit very seldom except in cases where we hit the error condition we're trying to monitor for.
                if (!result.Success || result.Key?.PrivKey?.P == 0 || result.Key?.PrivKey?.Q == 0)
                {
                    var serializerSettings = new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented,
                        Converters = new List<JsonConverter>
                        {
                            new BitstringBitLengthConverter(),
                            new DomainConverter(),
                            new BigIntegerConverter(),
                            new StringEnumConverter()
                        }
                    };

                    var jsonParam = JsonConvert.SerializeObject(copyParam, serializerSettings);
                    _logger.Warn($"KeyGen failed with the following parameter: {jsonParam}");

                    var jsonResult = JsonConvert.SerializeObject(result, serializerSettings);
                    _logger.Warn($"KeyGen result: {jsonResult}");
                }

            } while (!result.Success);

            return new RsaKeyResult
            {
                Key = result.Key,
                AuxValues = result.Aux,
                BitLens = copyParam.BitLens,
                Seed = copyParam.Seed
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

        public async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveV2_0Async(RsaSignaturePrimitiveParameters param)
        {
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = param.KeyFormat,
                Modulus = param.Modulus,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                PublicExponentMode = param.PublicExponent == null ? PublicExponentModes.Random : PublicExponentModes.Fixed,
                PublicExponent = param.PublicExponent,
                KeyMode = PrimeGenModes.RandomProbablePrimes,
                Standard = Fips186Standard.Fips186_4
            };

            var key = await GetRsaKeyAsync(keyParam);

            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverRsaSignaturePrimitiveV2_0CaseGrain, RsaSignaturePrimitiveResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaSignaturePrimitiveAsync(param);
            }
        }
        
        public async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            var keyParam = new RsaKeyParameters
            {
                KeyFormat = param.KeyFormat,
                Modulus = param.Modulus,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                PublicExponentMode = param.PublicExponent == null ? PublicExponentModes.Random : PublicExponentModes.Fixed,
                PublicExponent = param.PublicExponent,
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

        public async Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveSp800B56Br2Async(
            RsaDecryptionPrimitiveParameters param)
        {
            var keyParam = new RsaKeyParameters()
            {
                KeyFormat = param.Mode.Equals(PrivateKeyModes.Standard) ? PrivateKeyModes.Standard : PrivateKeyModes.Crt,
                Modulus = param.Modulo,
                PrimeTest = PrimeTestModes.TwoPow100ErrorBound,
                PublicExponentMode = param.PublicExponent == null ? PublicExponentModes.Random : PublicExponentModes.Fixed,
                PublicExponent = param.PublicExponent,
                KeyMode = PrimeGenModes.RandomProbablePrimes,
                Standard = Fips186Standard.Fips186_4
            };
            
            var keyResult = await GetRsaKeyAsync(keyParam);
            var key = new KeyResult(keyResult.Key, keyResult.AuxValues);
            
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverRsaCompleteDpSp800Br2CaseGrain, RsaDecryptionPrimitiveResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, key, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetRsaDecryptionPrimitiveSp800B56Br2Async(param);
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

