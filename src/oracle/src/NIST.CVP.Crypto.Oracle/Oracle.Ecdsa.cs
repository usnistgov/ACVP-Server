using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools;
using NIST.CVP.Pools.Enums;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Ecdsa;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EcdsaKeyResult> GetEcdsaKeyAsync(EcdsaKeyParameters param)
        {
            var poolBoy = new PoolBoy<EcdsaKeyResult>(_poolConfig);
            var poolResult = poolBoy.GetObjectFromPool(param, PoolTypes.ECDSA_KEY);
            if (poolResult != null)
            {
                return poolResult;
            }

            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EcdsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EcdsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<EcdsaKeyResult> CompleteDeferredEcdsaKeyAsync(EcdsaKeyParameters param, EcdsaKeyResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaCompleteDeferredKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EcdsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EcdsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<EcdsaKeyResult>> GetEcdsaKeyVerifyAsync(EcdsaKeyParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaVerifyKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<EcdsaKeyResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<EcdsaKeyResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<EcdsaSignatureResult> GetDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EcdsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EcdsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> CompleteDeferredEcdsaSignatureAsync(EcdsaSignatureParameters param, EcdsaSignatureResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaCompleteDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<EcdsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<EcdsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<EcdsaSignatureResult> GetEcdsaSignatureAsync(EcdsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EcdsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EcdsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<EcdsaSignatureResult>> GetEcdsaVerifyResultAsync(EcdsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEcdsaVerifySignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<EcdsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<EcdsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
