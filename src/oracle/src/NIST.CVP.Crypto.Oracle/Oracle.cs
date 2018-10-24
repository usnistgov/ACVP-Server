using NIST.CVP.Common.Oracle;
using NIST.CVP.Math;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle : IOracle
    {
        private const double GCM_FAIL_RATIO = .25;
        private const double XPN_FAIL_RATIO = .25;
        private const double CMAC_FAIL_RATIO = .25;
        private const double KEYWRAP_FAIL_RATIO = .2;
        private const double KMAC_FAIL_RATIO = .5;
        private const int RSA_PUBLIC_EXPONENT_BITS_MIN = 32;
        private const int RSA_PUBLIC_EXPONENT_BITS_MAX = 64;

        // TODO configurable concurrency through config file and/or GenValAppRunner parameter
        // Task scheduler should be an interim step until orleans is implemented, at which
        // point it will be managing tasks.  Currently, without limiting the tasks enqueued,
        // you can potentially cap out memory usage w/o being able to complete tasks.
        private static readonly LimitedConcurrencyLevelTaskScheduler _scheduler;
        private static readonly TaskFactory _taskFactory;
        private readonly IOptions<PoolConfig> _poolConfig;

        static Oracle()
        {
            _scheduler = new LimitedConcurrencyLevelTaskScheduler(3);
            _taskFactory = new TaskFactory(_scheduler);
        }

        public Oracle(
            IOptions<PoolConfig> poolConfig
        )
        {
            _poolConfig = poolConfig;
        }
    }
}
