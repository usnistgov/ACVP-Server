using System;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Helpers;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces;
using NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Exceptions;
using NLog;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Models
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
                await GrainInvokeRetryWrapper.WrapGrainCall(Grain.HeartbeatSubscribe, GrainObserverReference, LoadSheddingRetries);

                if (GrainObserver.IsFaulted)
                {
                    await GrainInvokeRetryWrapper.WrapGrainCall(Grain.Unsubscribe, GrainObserverReference, LoadSheddingRetries);
                    var exception = GrainObserver.GetException();

                    if (exception.GetType() != typeof(InitialValuesInvalidException))
                    {
                        _logger.Error(exception, exception.Message);
                    }

                    throw exception;
                }
            }

            var result = GrainObserver.GetResult();
            await GrainInvokeRetryWrapper.WrapGrainCall(Grain.Unsubscribe, GrainObserverReference, LoadSheddingRetries);
            return result;
        }

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    }
}
