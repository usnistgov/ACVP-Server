using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    /// <summary>
    /// Provides a means of Observing changes to an <see cref="IGrainObserver{TResult}"/>
    /// </summary>
    /// <typeparam name="TResult">The type that is returned when observed.</typeparam>
    public interface IGrainObservable<out TResult>
    {
        /// <summary>
        /// Subscribe an observer to an observable for updates.
        /// </summary>
        /// <param name="observer">The observer to subscribe</param>
        /// <returns></returns>
        Task Subscribe(IGrainObserver<TResult> observer);
        /// <summary>
        /// Unsubscribe an observer from an observable.
        /// </summary>
        /// <param name="observer">The observer to unsubscribe</param>
        /// <returns></returns>
        Task Unsubscribe(IGrainObserver<TResult> observer);
    }
}