namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    /// <summary>
    /// Provides a means of getting instances of an <see cref="IOracle"/>
    /// </summary>
    public interface IOracleFactory
    {
        /// <summary>
        /// Get an <see cref="IOracle"/> that can prioritize getting cached values from pools prior to falling back to
        /// the Orleans cluster.
        /// </summary>
        /// <param name="constructWithCacheUtilization">When true get crypto values from the cached pools
        /// prior to falling back to on demand crypto.  When false, always use on demand crypto.</param>
        /// <returns></returns>
        IOracle Get(bool constructWithCacheUtilization);
    }
}
