using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKdfDeferredCaseGrain, KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKdfCompleteDeferredCaseGrain, KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAnsiX963KdfCaseGrain, AnsiX963KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverIkeV1KdfCaseGrain, IkeV1KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverIkeV2KdfCaseGrain, IkeV2KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSnmpKdfCaseGrain, SnmpKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSrtpKdfCaseGrain, SrtpKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSshKdfCaseGrain, SshKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTlsKdfCaseGrain, TlsKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TpmKdfResult> GetTpmKdfCaseAsync()
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTpmKdfCaseGrain, TpmKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
