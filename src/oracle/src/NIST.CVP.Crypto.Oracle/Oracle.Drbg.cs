using NIST.CVP.Crypto.Common.DRBG;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces.Drbg;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverDrbgCaseGrain, DrbgResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
