using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces.Tdes;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<TdesResult> GetTdesCaseAsync(TdesParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTdesCaseGrain, TdesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetTdesCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<TdesResult>> GetTdesMctCaseAsync(TdesParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTdesMctCaseGrain, MctResult<TdesResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetTdesMctCaseAsync(param);
            }
        }

        public async Task<TdesResult> GetDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTdesDeferredCounterCaseGrain, TdesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredTdesCounterCaseAsync(param);
            }
        }

        public async Task<TdesResult> CompleteDeferredTdesCounterCaseAsync(CounterParameters<TdesParameters> param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTdesCompleteDeferredCounterCaseGrain, TdesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredTdesCounterCaseAsync(param);
            }
        }

        public async Task<CounterResult> ExtractIvsAsync(TdesParameters param, TdesResult fullParam)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverTdesCounterExtractIvsCaseGrain, CounterResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await ExtractIvsAsync(param, fullParam);
            }
        }
    }
}
