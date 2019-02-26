using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Crypto.Oracle.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.KeyWrap;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKeyWrapCaseGrain, KeyWrapResult>();
            await GrainInvokeRetryWrapper.WrapGrainCall(observableGrain.Grain.BeginWorkAsync, param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
