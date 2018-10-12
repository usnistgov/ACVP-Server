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
    }
}