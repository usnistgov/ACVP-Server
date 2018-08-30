using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Math;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Eddsa;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<EddsaKeyResult> GetEddsaKeyAsync(EddsaKeyParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EddsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EddsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<EddsaKeyResult> CompleteDeferredEddsaKeyAsync(EddsaKeyParameters param, EddsaKeyResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaCompleteDeferredKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EddsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EddsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<EddsaKeyResult>> GetEddsaKeyVerifyAsync(EddsaKeyParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaVerifyKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<EddsaKeyResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<EddsaKeyResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EddsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EddsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<EddsaSignatureResult>> CompleteDeferredEddsaSignatureAsync(EddsaSignatureParameters param, EddsaSignatureResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaCompleteDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<EddsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<EddsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<EddsaSignatureResult> GetEddsaSignatureAsync(EddsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EddsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EddsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<EddsaSignatureResult>> GetEddsaVerifyResultAsync(EddsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaVerifySignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<EddsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<EddsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        // These can be merged or removed depending on test group bit flip
        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetDeferredEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaDeferredSignatureBitFlipCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EddsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EddsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        // Remove or merge based on what we do with test group
        public async Task<EddsaSignatureResult> GetEddsaSignatureBitFlipAsync(EddsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaSignatureBitFlipCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<EddsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<EddsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        // Remove or do something... this is a little awkward how it is done
        public async Task<BitString> GetEddsaMessageBitFlipAsync(EddsaMessageParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverEddsaMessageBitFlipCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<BitString>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<BitString>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
