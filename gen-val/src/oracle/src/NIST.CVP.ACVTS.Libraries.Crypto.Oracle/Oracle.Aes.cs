using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Aes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<AesResult> GetAesCaseAsync(AesParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesCaseAsync(param);
            }
        }

        public virtual async Task<MctResult<AesResult>> GetAesMctCaseAsync(AesParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesMctCaseGrain, MctResult<AesResult>>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesMctCaseAsync(param);
            }
        }

        public async Task<AesXtsResult> GetAesXtsCaseAsync(AesXtsParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesXtsCaseGrain, AesXtsResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesXtsCaseAsync(param);
            }

        }

        public async Task<AesResult> GetAesFfCaseAsync(AesFfParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesFfCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesFfCaseAsync(param);
            }
        }

        public async Task<AesResult> GetDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesDeferredCounterCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDeferredAesCounterCaseAsync(param);
            }
        }

        public async Task<AesResult> CompleteDeferredAesCounterCaseAsync(CounterParameters<AesParameters> param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesCompleteDeferredCounterCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredAesCounterCaseAsync(param);
            }
        }

        public async Task<CounterResult> ExtractIvsAsync(AesParameters param, AesResult fullParam)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesCounterExtractIvsCaseGrain, CounterResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, fullParam, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await ExtractIvsAsync(param, fullParam);
            }
        }

        public async Task<AesResult> GetAesCounterRfcCaseAsync(CounterParameters<AesParameters> param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverAesCounterRfcCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesCounterRfcCaseAsync(param);
            }
        }

        public async Task<AesResult> CompleteDeferredAesCounterRfcCaseAsync(AesWithPayloadParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IObserverCompleteDeferredAesCounterRfcCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await CompleteDeferredAesCounterRfcCaseAsync(param);
            }
        }

        public async Task<AesResult> GetAesCaseAsync(AesWithPayloadParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverAesWithPayloadCaseGrain, AesResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetAesCaseAsync(param);
            }
        }
    }
}
