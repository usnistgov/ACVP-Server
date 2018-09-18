using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Rsa;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        //private RsaPrimeResult GeneratePrimes(RsaKeyParameters param, IEntropyProvider entropyProvider)
        //{
        //    // Only works with random public exponent
        //    var poolBoy = new PoolBoy<RsaPrimeResult>(_poolConfig);
        //    var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.RSA_KEY);
        //    if (poolResult != null)
        //    {
        //        return poolResult;
        //    }

        //    // TODO Not every group has a hash alg... Can use a default value perhaps?
        //    ISha sha = null;
        //    if (param.HashAlg != null)
        //    {
        //        sha = _shaFactory.GetShaInstance(param.HashAlg);
        //    }

        //    var keyComposer = _keyComposerFactory.GetKeyComposer(param.KeyFormat);

        //    // Configure Prime Generator
        //    var keyResult = new KeyBuilder(new PrimeGeneratorFactory())
        //        .WithBitlens(param.BitLens)
        //        .WithEntropyProvider(entropyProvider)
        //        .WithHashFunction(sha)
        //        .WithNlen(param.Modulus)
        //        .WithPrimeGenMode(param.KeyMode)
        //        .WithPrimeTestMode(param.PrimeTest)
        //        .WithPublicExponent(param.PublicExponent)
        //        .WithKeyComposer(keyComposer)
        //        .WithSeed(param.Seed)
        //        .Build();

        //    return new RsaPrimeResult
        //    {
        //        Aux = keyResult.AuxValues,
        //        Key = keyResult.Key,
        //        Success = keyResult.Success
        //    };
        //}

        public async Task<RsaKeyResult> GetRsaKeyAsync(RsaKeyParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaKeyResult> CompleteKeyAsync(RsaKeyResult param, PrivateKeyModes keyMode)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaCompleteKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, keyMode);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaKeyResult> CompleteDeferredRsaKeyCaseAsync(RsaKeyParameters param, RsaKeyResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaCompleteDeferredKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<RsaKeyResult>> GetRsaKeyVerifyAsync(RsaKeyResult param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaVerifyKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<RsaKeyResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<RsaKeyResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaSignaturePrimitiveResult> GetRsaSignaturePrimitiveAsync(RsaSignaturePrimitiveParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaSignaturePrimitiveCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaSignaturePrimitiveResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaSignaturePrimitiveResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaSignatureResult> GetDeferredRsaSignatureAsync(RsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<RsaSignatureResult>> CompleteDeferredRsaSignatureAsync(RsaSignatureParameters param, RsaSignatureResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaCompleteDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<RsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<RsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaSignatureResult> GetRsaSignatureAsync(RsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<RsaSignatureResult>> GetRsaVerifyAsync(RsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaVerifySignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<RsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<RsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaDecryptionPrimitiveResult> GetDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaDeferredDecryptionPrimitiveCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaDecryptionPrimitiveResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaDecryptionPrimitiveResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaDecryptionPrimitiveResult> CompleteDeferredRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param,
            RsaDecryptionPrimitiveResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaCompleteDeferredDecryptionPrimitiveCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaDecryptionPrimitiveResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaDecryptionPrimitiveResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<RsaDecryptionPrimitiveResult> GetRsaDecryptionPrimitiveAsync(RsaDecryptionPrimitiveParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverRsaDecryptionPrimitiveCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<RsaDecryptionPrimitiveResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<RsaDecryptionPrimitiveResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}

