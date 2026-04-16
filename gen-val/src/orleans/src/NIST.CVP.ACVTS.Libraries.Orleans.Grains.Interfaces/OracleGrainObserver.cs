using System;
using System.Threading;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces
{
    public class OracleGrainObserver<TResult> : IGrainObserver<TResult>, IObserverResult<TResult>
    {
        public bool HasResult => _hasResult;
        public bool IsFaulted => _isFaulted;

        private volatile bool _hasResult;
        private volatile bool _isFaulted;
        
        private TResult _result;
        private Exception _exception;

        public TResult GetResult()
        {
            return _result;
        }

        public Exception GetException()
        {
            return _exception;
        }

        public void ReceiveMessageFromCluster(TResult result)
        {
            _result = result;
            
            // Force the previous statement to complete before declaring the result is ready
            Thread.MemoryBarrier();
            
            _hasResult = true;
        }

        public void Throw(Exception exception)
        {
            _exception = exception;
            
            // Force the previous statement to complete before declaring an exception is present
            Thread.MemoryBarrier();
            
            _isFaulted = true;
        }
    }
}
