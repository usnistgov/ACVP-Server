using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<XecdhKeyResult> GetXecdhKeyAsync(XecdhKeyParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverXecdhKeyCaseGrain, XecdhKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetXecdhKeyAsync(param);
            }
        }

        public async Task<XecdhKeyResult> CompleteDeferredXecdhKeyAsync(XecdhKeyParameters param, XecdhKeyResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverXecdhCompleteDeferredKeyCaseGrain, XecdhKeyResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredXecdhKeyAsync(param, fullParam);
            }
        }

        public async Task<VerifyResult<XecdhKeyResult>> GetXecdhKeyVerifyAsync(XecdhKeyParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverXecdhVerifyKeyCaseGrain, VerifyResult<XecdhKeyResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetXecdhKeyVerifyAsync(param);
            }
        }

        public async Task<XecdhSscResult> GetXecdhSscTestAsync(XecdhSscParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverXecdhSscCaseGrain, XecdhSscResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetXecdhSscTestAsync(param);
            }
        }

        public async Task<XecdhSscDeferredResult> CompleteDeferredXecdhSscTestAsync(XecdhSscDeferredParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverXecdhCompleteDeferredSscCaseGrain, XecdhSscDeferredResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredXecdhSscTestAsync(param);
            }
        }
    }
}
