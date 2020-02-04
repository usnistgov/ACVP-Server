using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces.Kdf;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KdfResult> GetDeferredKdfCaseAsync(KdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverKdfDeferredCaseGrain, KdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredKdfCaseAsync(param);
            }
        }

        public async Task<KdfResult> CompleteDeferredKdfCaseAsync(KdfParameters param, KdfResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverKdfCompleteDeferredCaseGrain, KdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredKdfCaseAsync(param, fullParam);
            }
        }

        public async Task<AnsiX963KdfResult> GetAnsiX963KdfCaseAsync(AnsiX963Parameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAnsiX963KdfCaseGrain, AnsiX963KdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAnsiX963KdfCaseAsync(param);
            }
        }

        public async Task<AnsiX942KdfResult> GetAnsiX942KdfCaseAsync(AnsiX942Parameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAnsiX942KdfCaseGrain, AnsiX942KdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAnsiX942KdfCaseAsync(param);
            }
        }

        public async Task<IkeV1KdfResult> GetIkeV1KdfCaseAsync(IkeV1KdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverIkeV1KdfCaseGrain, IkeV1KdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetIkeV1KdfCaseAsync(param);
            }
        }

        public async Task<IkeV2KdfResult> GetIkeV2KdfCaseAsync(IkeV2KdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverIkeV2KdfCaseGrain, IkeV2KdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetIkeV2KdfCaseAsync(param);
            }
        }
        
        public async Task<PbKdfResult> GetPbKdfCaseAsync(PbKdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverPbKdfCaseGrain, PbKdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetPbKdfCaseAsync(param);
            }
        }

        public async Task<SnmpKdfResult> GetSnmpKdfCaseAsync(SnmpKdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverSnmpKdfCaseGrain, SnmpKdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetSnmpKdfCaseAsync(param);
            }
        }

        public async Task<SrtpKdfResult> GetSrtpKdfCaseAsync(SrtpKdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverSrtpKdfCaseGrain, SrtpKdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetSrtpKdfCaseAsync(param);
            }
        }

        public async Task<SshKdfResult> GetSshKdfCaseAsync(SshKdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverSshKdfCaseGrain, SshKdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetSshKdfCaseAsync(param);
            }
        }

        public async Task<TlsKdfResult> GetTlsKdfCaseAsync(TlsKdfParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTlsKdfCaseGrain, TlsKdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetTlsKdfCaseAsync(param);
            }
        }

        public async Task<TpmKdfResult> GetTpmKdfCaseAsync()
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTpmKdfCaseGrain, TpmKdfResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, "No params for TPM KDF.");
                return await GetTpmKdfCaseAsync();
            }
        }
    }
}
