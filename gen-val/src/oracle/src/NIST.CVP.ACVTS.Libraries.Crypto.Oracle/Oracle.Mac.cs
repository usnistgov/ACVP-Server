using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverCmacCaseGrain, MacResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetCmacCaseAsync(param);
            }
        }

        public async Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverHmacCaseGrain, MacResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetHmacCaseAsync(param);
            }
        }

        public async Task<KmacResult> GetKmacCaseAsync(KmacParameters param)
        {
            try
            {
                var observableGrain =
                    await GetObserverGrain<IOracleObserverKmacCaseGrain, KmacResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                _logger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKmacCaseAsync(param);
            }
        }
    }
}
