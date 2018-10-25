using System;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IObserverResult<out TResult>
    {
        bool HasResult { get; }
        bool IsFaulted { get; }
        TResult GetResult();
        Exception GetException();
    }
}