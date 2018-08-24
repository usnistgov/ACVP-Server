using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    /// <summary>
    /// Provides a means of Observing changes to an <see cref="IGrainObserver{TResult}"/>
    /// </summary>
    /// <typeparam name="TResult">The type that is returned when observed.</typeparam>
    public interface IGrainObservable<out TResult>
    {
        Task Subscribe(IGrainObserver<TResult> observer);
        Task Unsubscribe(IGrainObserver<TResult> observer);
    }
}