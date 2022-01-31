using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces
{
    /// <summary>
    /// Provides a means of Observing changes to an <see cref="IGrainObserver{TResult}"/>
    /// </summary>
    /// <typeparam name="TResult">The type that is returned when observed.</typeparam>
    public interface IGrainObservable<out TResult>
    {
        /// <summary>
        /// The original subscribe from observer to observable, bootstraps some values to monitor for node suicide.
        /// </summary>
        /// <param name="observer">The observer to subscribe.</param>
        /// <returns></returns>
        Task InitialSubscribe(IGrainObserver<TResult> observer);
        /// <summary>
        /// Heartbeat Subscribe refreshes observer to an observable to prevent staleness.
        /// </summary>
        /// <param name="observer">The observer to subscribe.</param>
        /// <returns></returns>
        Task HeartbeatSubscribe(IGrainObserver<TResult> observer);
        /// <summary>
        /// Unsubscribe an observer from an observable.
        /// </summary>
        /// <param name="observer">The observer to unsubscribe.</param>
        /// <returns></returns>
        Task Unsubscribe(IGrainObserver<TResult> observer);
    }
}
