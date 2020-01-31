using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Orleans.Grains.Interfaces.Aead;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AeadResult> GetAesCcmCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesCcmCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesCcmCaseAsync(param);
            }
        }
        
        public async Task<AeadResult> GetEcmaCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesCcmEcmaCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetEcmaCaseAsync(param);
            }
        }

        public async Task<AeadResult> GetAesGcmCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesGcmCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesGcmCaseAsync(param);
            }
        }

        public async Task<AeadResult> GetAesGcmSivCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesGcmSivCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesGcmSivCaseAsync(param);
            }
        }

        public async Task<AeadResult> GetAesXpnCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesXpnCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesXpnCaseAsync(param);
            }
        }
        
        public async Task<AeadResult> GetDeferredAesGcmCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesDeferredGcmCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredAesGcmCaseAsync(param);
            }
        }

        public async Task<AeadResult> CompleteDeferredAesGcmCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesCompleteDeferredGcmCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredAesGcmCaseAsync(param, fullParam);
            }
        }

        public async Task<AeadResult> GetDeferredAesXpnCaseAsync(AeadParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesDeferredXpnCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredAesXpnCaseAsync(param);
            }
        }

        public async Task<AeadResult> CompleteDeferredAesXpnCaseAsync(AeadParameters param, AeadResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverAesCompleteDeferredXpnCaseGrain, AeadResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredAesXpnCaseAsync(param, fullParam);
            }
        }
    }
}
