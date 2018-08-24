using System.Threading.Tasks;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public class OracleGrainObserver<TResult> : IGrainObserver<TResult>, IObserverResult<TResult>
    {
        private TResult _result;
        public bool HasResult { get; private set; }

        public TResult GetResult()
        {
            return _result;
        }

        public void ReceiveMessageFromCluster(TResult result)
        {
            HasResult = true;
            _result = result;
        }
    }
}