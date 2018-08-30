using System;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.CSHAKE;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Orleans.Grains.Interfaces;
using NIST.CVP.Orleans.Grains.Interfaces.Helpers;
using NIST.CVP.Orleans.Grains.Interfaces.Mac;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public async Task<MacResult> GetCmacCaseAsync(CmacParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverCmacCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MacResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MacResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<MacResult> GetHmacCaseAsync(HmacParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverHmacCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<MacResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<MacResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }

        public async Task<KmacResult> GetKmacCaseAsync(KmacParameters param)
        {
            var grain = _clusterClient.GetGrain<IOracleObserverKmacCaseGrain>(
                Guid.NewGuid()
            );

            var observer = new OracleGrainObserver<KmacResult>();
            var observerReference = 
                await _clusterClient.CreateObjectReference<IGrainObserver<KmacResult>>(observer);
            await grain.Subscribe(observerReference);
            await grain.BeginWorkAsync(param);

            var result = await ObservableHelpers.ObserveUntilResult(grain, observer, observerReference);

            return result;
        }
    }
}
