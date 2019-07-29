using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverKdfDeferredCaseGrain, KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverKdfCompleteDeferredCaseGrain, KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAnsiX963KdfCaseGrain, AnsiX963KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AnsiX942KdfResult> GetAnsiX942KdfCaseAsync(AnsiX942Parameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverAnsiX942KdfCaseGrain, AnsiX942KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverIkeV1KdfCaseGrain, IkeV1KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverIkeV2KdfCaseGrain, IkeV2KdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
        
        public async Task<PbKdfResult> GetPbKdfCaseAsync(PbKdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverPbKdfCaseGrain, PbKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverSnmpKdfCaseGrain, SnmpKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverSrtpKdfCaseGrain, SrtpKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverSshKdfCaseGrain, SshKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTlsKdfCaseGrain, TlsKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TpmKdfResult> GetTpmKdfCaseAsync()
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverTpmKdfCaseGrain, TpmKdfResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
