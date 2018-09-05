using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.KeyWrap;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKeyWrapCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KeyWrapResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KeyWrapResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
