using Microsoft.Extensions.Logging;

namespace NIST.CVP.ACVTS.Libraries.Common.Config
{
    /// <summary>
    /// Configuration class for Orleans
    /// </summary>
    public class OrleansConfig
    {
        /// <summary>
        /// Orleans per node information.
        /// </summary>
        public OrleansNodeConfig[] OrleansNodeConfig { get; set; }

        /// <summary>
        /// Intra-silo communication port.
        /// </summary>
        public int OrleansSiloPort { get; set; }

        /// <summary>
        /// Silo to client communication port.
        /// </summary>
        public int OrleansGatewayPort { get; set; }

        /// <summary>
        /// The name to use for the cluster
        /// </summary>
        public string ClusterId { get; set; }

        /// <summary>
        /// The minimum level in which to log
        /// </summary>
        public LogLevel MinimumLogLevel { get; set; }

        /// <summary>
        /// The port to use for the orleans dashboard
        /// </summary>
        public int OrleansDashboardPort { get; set; }

        /// <summary>
        /// A fall back value in case the node cannot be mapped to the configuration.
        /// This value will be used by the task scheduler for a maximum level of concurrency.
        /// </summary>
        public int FallBackMinimumCores { get; set; }

        /// <summary>
        /// The CPU threshold in which the cluster will start rejecting additional requests.
        /// The rejected requests throw an exception
        /// </summary>
        public int LoadSheddingCpuThreshold { get; set; }

        /// <summary>
        /// The maximum number of concurrent work enqueued to Orleans work per GenVal instance.
        /// </summary>
        public int MaxWorkItemsToQueuePerGenValInstance { get; set; }

        /// <summary>
        /// The maximum amount of times to retry a grain request in cases of timeout or load shedding.
        /// </summary>
        public int TimeoutRetryAttempts { get; set; }
    }
}
