using System;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Oracle.Helpers;
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
        public int LoadSheddingRetries { get; }

        public OracleObserverGrain(
            TGrain grain, 
            OracleGrainObserver<TGrainResultType> grainObserver, 
            IGrainObserver<TGrainResultType> grainObserverReference,
            int loadSheddingRetries
        )
        {
            Grain = grain;
            GrainObserver = grainObserver;
            GrainObserverReference = grainObserverReference;
            LoadSheddingRetries = loadSheddingRetries;
        }

        public async Task<TGrainResultType> ObserveUntilResult()
        {
            while (!GrainObserver.HasResult)
            {
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                await GrainInvokeRetryWrapper.WrapGrainCall(Grain.Subscribe, GrainObserverReference, LoadSheddingRetries);

                if (GrainObserver.IsFaulted)
                {
                    await GrainInvokeRetryWrapper.WrapGrainCall(Grain.Unsubscribe, GrainObserverReference, LoadSheddingRetries);
                    throw GrainObserver.GetException();
                }
            }

            var result = GrainObserver.GetResult();
            await GrainInvokeRetryWrapper.WrapGrainCall(Grain.Unsubscribe, GrainObserverReference, LoadSheddingRetries);
            return result;
        }
    }
}