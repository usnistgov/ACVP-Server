namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IObserverResult<out TResult>
    {
        bool HasResult { get; }
        TResult GetResult();
    }
}