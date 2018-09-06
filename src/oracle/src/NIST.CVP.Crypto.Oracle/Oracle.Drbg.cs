using System;
using NIST.CVP.Crypto.Common.DRBG;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Drbg;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using DrbgResult = NIST.CVP.Common.Oracle.ResultTypes.DrbgResult;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<DrbgResult> GetDrbgCaseAsync(DrbgParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverDrbgCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<DrbgResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<DrbgResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
