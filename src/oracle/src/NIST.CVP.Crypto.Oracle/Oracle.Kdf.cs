using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKdfDeferredCaseGrain, KdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKdfCompleteDeferredCaseGrain, KdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param, fullParam);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverAnsiX963KdfCaseGrain, AnsiX963KdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverIkeV1KdfCaseGrain, IkeV1KdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverIkeV2KdfCaseGrain, IkeV2KdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSnmpKdfCaseGrain, SnmpKdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSrtpKdfCaseGrain, SrtpKdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverSshKdfCaseGrain, SshKdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTlsKdfCaseGrain, TlsKdfResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<TpmKdfResult> GetTpmKdfCaseAsync()
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverTpmKdfCaseGrain, TpmKdfResult>();
            await observableGrain.Grain.BeginWorkAsync();

            return await observableGrain.ObserveUntilResult();
        }
    }
}
