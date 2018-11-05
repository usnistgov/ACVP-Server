using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.ExtensionMethods;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverCmacCaseGrain, MacResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverHmacCaseGrain, MacResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }

        public async Task<KmacResult> GetKmacCaseAsync(KmacParameters param)
        {
            var observableGrain = 
                await _clusterClient.GetObserverGrain<IOracleObserverKmacCaseGrain, KmacResult>();
            await observableGrain.Grain.BeginWorkAsync(param);

            return await observableGrain.ObserveUntilResult();
        }
    }
}
