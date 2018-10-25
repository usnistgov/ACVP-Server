using System;
using System.Threading.Tasks;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    /// <summary>
    /// Orleans observer interface, allows the orleans cluster to convey observations to clients.
    /// </summary>
    /// <typeparam name="TResult">The type of value observed.</typeparam>
    public interface IGrainObserver<in TResult> : IGrainObserver
    {
        void ReceiveMessageFromCluster(TResult result);
        void Throw(Exception exception);
    }
}