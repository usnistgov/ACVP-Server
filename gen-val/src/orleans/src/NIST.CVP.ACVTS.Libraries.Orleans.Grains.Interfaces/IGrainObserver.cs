using System;
using System.Threading.Tasks;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces
{
    /// <summary>
    /// Orleans observer interface, allows the orleans cluster to convey observations to clients.
    /// </summary>
    /// <typeparam name="TResult">The type of value observed.</typeparam>
    public interface IGrainObserver<in TResult> : IGrainObserver
    {
        /// <summary>
        /// Push a <see cref="TResult"/> to its observers
        /// </summary>
        /// <param name="result"></param>
        void ReceiveMessageFromCluster(TResult result);
        /// <summary>
        /// Pushes an <see cref="exception"/> to its observers.  Grain is in a faulted state.
        /// </summary>
        /// <param name="exception"></param>
        void Throw(Exception exception);
    }
}
