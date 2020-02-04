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
        /// Can be used to override the minimum pool level for *all* pools.  Mostly useful for development/debugging purposes.
        /// </summary>
        public int OverrideMinimumPoolLevel { get; set; }

        /// <summary>
        /// When a pool value is used, should it be logged?
        /// </summary>
        public bool ShouldLogPoolValueUse { get; set; }

        /// <summary>
        /// Logs up to string length of the used pool value when <see cref="ShouldLogPoolValueUse"/> is true.
        /// <remarks>Use 0 to log the entirety of the pool value (this will get big for things like MCT pool values).</remarks>
        /// </summary>
        public int PoolResultLogLength { get; set; }
        
        /// <summary>
        /// Should the pools be allowed to spawn from the controller
        /// </summary>
        public bool AllowPoolSpawn { get; set; }
    }
}