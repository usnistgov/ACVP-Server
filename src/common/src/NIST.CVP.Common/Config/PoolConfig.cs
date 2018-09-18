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
    }
}