using NIST.CVP.Crypto.Oracle.Models;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.Oracle.ExtensionMethods
{
    public static class ClusterClientExtensionMethods
    {
        public static async Task<OracleObserverGrain<TGrain, TGrainResultType>> 
            GetObserverGrain<TGrain, TGrainResultType>(this IClusterClient client)
            where TGrain : IGrainObservable<TGrainResultType>, IGrainWithGuidKey
        {
            var grain = client.GetGrain<TGrain>(Guid.NewGuid());
            
            var observer = new OracleGrainObserver<TGrainResultType>();
            var observerReference = 
                await client.CreateObjectReference<IGrainObserver<TGrainResultType>>(observer);
            await grain.Subscribe(observerReference);

            return new OracleObserverGrain<TGrain, TGrainResultType>(grain, observer, observerReference);
        }
    }
}