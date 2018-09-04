using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.ANSIX963;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.IKEv1;
using NIST.CVP.Crypto.IKEv2;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SNMP;
using NIST.CVP.Crypto.SRTP;
using NIST.CVP.Crypto.SSH;
using NIST.CVP.Crypto.TLS;
using NIST.CVP.Math;
using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKdfDeferredCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKdfCompleteDeferredCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param, fullParam);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverAnsiX963KdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<AnsiX963KdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<AnsiX963KdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverIkeV1KdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<IkeV1KdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<IkeV1KdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverIkeV2KdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<IkeV2KdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<IkeV2KdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverSnmpKdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<SnmpKdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<SnmpKdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverSrtpKdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<SrtpKdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<SrtpKdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverSshKdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<SshKdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<SshKdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverTlsKdfCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<TlsKdfResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<TlsKdfResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
