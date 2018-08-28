using System;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces.Helpers
{
    public class ObservableHelpers
    {
        /// <summary>
        /// Observes a grain observable until a result is present, returns said result
        /// and unsubscribes grain observer.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="grain">The grain in to subscribe to</param>
        /// <param name="observer">The observer</param>
        /// <param name="observerReference">The orleans cluster's observer reference</param>
        /// <returns></returns>
        public static async Task<TResult> ObserveUntilResult<TResult>(
            IGrainObservable<TResult> grain, 
            IObserverResult<TResult> observer, 
            IGrainObserver<TResult> observerReference
        )
        {
            while (!observer.HasResult)
            {
                await Task.Delay(TimeSpan.FromSeconds(Constants.TaskPollingSeconds));
                await grain.Subscribe(observerReference);
            }

            var result = observer.GetResult();
            await grain.Unsubscribe(observerReference);
            return result;
        }
    }
}