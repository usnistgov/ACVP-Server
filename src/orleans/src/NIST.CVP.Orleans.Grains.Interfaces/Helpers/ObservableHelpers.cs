using System;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces.Helpers
{
    public class ObservableHelpers
    {
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