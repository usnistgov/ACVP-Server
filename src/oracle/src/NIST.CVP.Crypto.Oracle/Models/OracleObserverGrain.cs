using System;
using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces;
using Orleans;

namespace NIST.CVP.Crypto.Oracle.Models
{
    public class OracleObserverGrain<TGrain, TGrainResultType>
        where TGrain : IGrainObservable<TGrainResultType>, IGrainWithGuidKey
    {
        public TGrain Grain { get; }
        public OracleGrainObserver<TGrainResultType> GrainObserver { get; }
        public IGrainObserver<TGrainResultType> GrainObserverReference { get; }

        public OracleObserverGrain(
            TGrain grain, 
            OracleGrainObserver<TGrainResultType> grainObserver, 
            IGrainObserver<TGrainResultType> grainObserverReference
        )
        {
            Grain = grain;
            GrainObserver = grainObserver;
            GrainObserverReference = grainObserverReference;
        }

        public async Task<TGrainResultType> ObserveUntilResult()
        {
            while (!GrainObserver.HasResult)
            {
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                await Grain.Subscribe(GrainObserverReference);

                if (GrainObserver.IsFaulted)
                {
                    await Grain.Unsubscribe(GrainObserverReference);
                    throw GrainObserver.GetException();
                }
            }

            var result = GrainObserver.GetResult();
            await Grain.Unsubscribe(GrainObserverReference);
            return result;
        }
    }
}