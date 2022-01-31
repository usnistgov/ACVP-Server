using System;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces
{
    /// <summary>
    /// Interface used for interacting with an observable
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IObserverResult<out TResult>
    {
        /// <summary>
        /// Does the observer have a result? (used in polling)
        /// </summary>
        bool HasResult { get; }
        /// <summary>
        /// Is the observable in a faulted state?
        /// </summary>
        bool IsFaulted { get; }
        /// <summary>
        /// Get the <see cref="TResult"/> from the observable.
        /// </summary>
        /// <returns></returns>
        TResult GetResult();
        /// <summary>
        /// Get the exception from the observable.
        /// </summary>
        /// <returns></returns>
        Exception GetException();
    }
}
