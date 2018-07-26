using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private readonly Random800_90 _rand = new Random800_90();

        private const double GCM_FAIL_RATIO = .25;
        private const double XPN_FAIL_RATIO = .25;
        private const double CMAC_FAIL_RATIO = .25;
        private const double KEYWRAP_FAIL_RATIO = .2;

        // TODO configurable concurrency through config file and/or GenValAppRunner parameter
        // Task scheduler should be an interim step until orleans is implemented, at which
        // point it will be managing tasks.  Currently, without limiting the tasks enqueued,
        // you can potentially cap out memory usage w/o being able to complete tasks.
        private static readonly LimitedConcurrencyLevelTaskScheduler _scheduler;
        private static readonly TaskFactory _taskFactory;

        static Oracle()
        {
            _scheduler = new LimitedConcurrencyLevelTaskScheduler(3);
            _taskFactory = new TaskFactory(_scheduler);
        }
    }
}
