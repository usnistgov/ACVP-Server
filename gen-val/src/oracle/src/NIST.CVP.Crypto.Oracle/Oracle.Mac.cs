using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.Crypto.Oracle
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
