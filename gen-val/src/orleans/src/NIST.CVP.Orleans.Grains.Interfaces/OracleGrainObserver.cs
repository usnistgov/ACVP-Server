using System;
using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public class OracleGrainObserver<TResult> : IGrainObserver<TResult>, IObserverResult<TResult>
    {
        public bool HasResult { get; private set; }
        public bool IsFaulted { get; private set; }

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
            HasResult = true;
            _result = result;
        }

        public void Throw(Exception exception)
        {
            IsFaulted = true;
            _exception = exception;
        }
    }
}