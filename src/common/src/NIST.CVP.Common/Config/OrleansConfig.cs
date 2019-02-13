using Microsoft.Extensions.Logging;

namespace NIST.CVP.Common.Config
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
        /// Should logs be written to console?
        /// </summary>
        public bool UseConsoleLogging { get; set; }
        
        /// <summary>
        /// Should logs be written to a file?
        /// </summary>
        public bool UseFileLogging { get; set; }

        /// <summary>
        /// The post to use for the orleans dashboard
        /// </summary>
        public int OrleansDashboardPort {get; set; }
    }
}