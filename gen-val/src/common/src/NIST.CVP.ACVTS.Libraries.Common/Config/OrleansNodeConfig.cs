namespace NIST.CVP.ACVTS.Libraries.Common.Config
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
        /// The maximum amount of simultaneous work taken on by the node in the cluster.
        /// Value should be *no more* than the total number of processors on the node minus 2.
        /// </summary>
        public int MaxConcurrentWork { get; set; } = 2;
    }
}
