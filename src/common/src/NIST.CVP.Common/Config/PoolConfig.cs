namespace NIST.CVP.Common.Config
{
    /// <summary>
    /// Configuration values for pool endpoint
    /// </summary>
    public class PoolConfig
    {
        /// <summary>
        /// The root location of the pool manager
        /// </summary>
        public string RootUrl { get; set; }
        
        /// <summary>
        /// The pool managers port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Should the precomputed value be pushed back onto the queue once used?
        /// </summary>
        public bool ShouldRecyclePoolWater { get; set; }

        /// <summary>
        /// The file to use for pool configuration.
        /// </summary>
        public string PoolConfigFile { get; set; }

        /// <summary>
        /// The directory where the pool water files are stored.
        /// </summary>
        public string PoolDirectory { get; set; }
    }
}