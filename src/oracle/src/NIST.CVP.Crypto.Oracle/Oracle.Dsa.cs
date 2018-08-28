using NIST.CVP.Common.Helpers;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<DsaDomainParametersResult> GetDsaPQAsync(DsaDomainParametersParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaPqCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<DsaDomainParametersResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<DsaDomainParametersResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<DsaDomainParametersResult> GetDsaGAsync(DsaDomainParametersParameters param, DsaDomainParametersResult pqParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaGCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<DsaDomainParametersResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<DsaDomainParametersResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, pqParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<DsaDomainParametersResult> GetDsaDomainParametersAsync(DsaDomainParametersParameters param)
        {
            var pqResult = await GetDsaPQAsync(param);
            var gResult = await GetDsaGAsync(param, pqResult);

            return new DsaDomainParametersResult
            {
                Counter = pqResult.Counter,
                G = gResult.G,
                H = gResult.H,
                Index = gResult.Index,
                P = pqResult.P,
                Q = pqResult.Q,
                Seed = pqResult.Seed
            };
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaPQVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaPqVerifyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<DsaDomainParametersResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<DsaDomainParametersResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<DsaDomainParametersResult>> GetDsaGVerifyAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaGVerifyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<DsaDomainParametersResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<DsaDomainParametersResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<DsaKeyResult> GetDsaKeyAsync(DsaKeyParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<DsaKeyResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<DsaKeyResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<DsaKeyResult>> CompleteDeferredDsaKeyAsync(DsaKeyParameters param, DsaKeyResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaCompleteDeferredKeyCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<DsaKeyResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<DsaKeyResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<DsaSignatureResult> GetDeferredDsaSignatureAsync(DsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<DsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<DsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<VerifyResult<DsaSignatureResult>> CompleteDeferredDsaSignatureAsync(DsaSignatureParameters param, DsaSignatureResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaCompleteDeferredSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<VerifyResult<DsaSignatureResult>>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<VerifyResult<DsaSignatureResult>>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<DsaSignatureResult> GetDsaSignatureAsync(DsaSignatureParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDsaSignatureCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<DsaSignatureResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<DsaSignatureResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
