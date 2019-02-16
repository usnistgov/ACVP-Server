namespace NIST.CVP.Common.Config
{
    /// <summary>
    /// Configuration for individual Orleans nodes.
    /// </summary>
    public class OrleansNodeConfig
    {
        /// <summary>
        /// The IP address/host of the node.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// The number of CPU cores the node has available.
        /// </summary>
        public int NumberOfCores { get; set; } = 2;
    }
}