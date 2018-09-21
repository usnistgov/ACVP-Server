using Microsoft.Extensions.Logging;

namespace NIST.CVP.Common.Config
{
    public class OrleansConfig
    {
        public string OrleansServerIp { get; set; }
        public int OrleansSiloPort { get; set; }
        public int OrleansGatewayPort { get; set; }
        public string ClusterId { get; set; }
        public LogLevel MinimumLogLevel { get; set; }
        public bool UseConsoleLogging { get; set; }
        public bool UseFileLogging { get; set;}
    }
}