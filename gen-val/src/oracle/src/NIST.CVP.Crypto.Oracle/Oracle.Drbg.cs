using System;
using NIST.CVP.Crypto.Common.DRBG;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Drbg;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverDrbgCaseGrain, DrbgResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetDrbgCaseAsync(param);
            }
        }
    }
}
