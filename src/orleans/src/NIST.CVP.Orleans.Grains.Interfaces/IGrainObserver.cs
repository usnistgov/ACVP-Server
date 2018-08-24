using System;
using System.Threading.Tasks;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IGrainObserver<in TResult> : IGrainObserver
    {
        void ReceiveMessageFromCluster(TResult result);
    }
}