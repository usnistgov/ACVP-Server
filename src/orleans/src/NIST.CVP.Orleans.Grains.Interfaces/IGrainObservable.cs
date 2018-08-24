using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IGrainObservable<TResult>
    {
        Task Subscribe(IGrainObserver<TResult> observer);
        Task Unsubscribe(IGrainObserver<TResult> observer);
    }
}