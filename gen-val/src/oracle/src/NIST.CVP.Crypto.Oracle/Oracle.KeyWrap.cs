using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Exceptions;
using NIST.CVP.Orleans.Grains.Interfaces.KeyWrap;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param)
        {
            try
            {
                var observableGrain = 
                    await GetObserverGrain<IOracleObserverKeyWrapCaseGrain, KeyWrapResult>();
                await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

                return await observableGrain.ObserveUntilResult();
            }
            catch (OriginalClusterNodeSuicideException ex)
            {
                ThisLogger.Warn(ex, $"{ex.Message}{Environment.NewLine}Restarting grain with {param.GetType()} parameter: {JsonConvert.SerializeObject(param)}");
                return await GetKeyWrapCaseAsync(param);
            }
        }
    }
}
