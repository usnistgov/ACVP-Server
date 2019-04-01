using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverCmacCaseGrain, MacResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverHmacCaseGrain, MacResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KmacResult> GetKmacCaseAsync(KmacParameters param)
        {
            var observableGrain = 
                await GetObserverGrain<IOracleObserverKmacCaseGrain, KmacResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param, LoadSheddingRetries);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
