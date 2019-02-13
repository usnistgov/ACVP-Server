namespace NIST.CVP.Pools.Enums
{
    /// <summary>
    /// The types of events that are logged in the pool log
    /// </summary>
    public enum LogTypes
    {
        /// <summary>
        /// The pool didn't have any/enough values to serve a precomputed value.
        /// </summary>
        PoolTooEmpty,
        /// <summary>
        /// No pool exists given the provided parameters.
        /// </summary>
        NoPool,
        /// <summary>
        /// A value was pulled from the pool.
        /// </summary>
        GetValueFromPool,
        /// <summary>
        /// A unit of work was dispatched to the Orleans cluster.
        /// </summary>
        QueueOrleansWorkToPool,
        /// <summary>
        /// Gives information on the current pool levels within each pool
        /// TODO
        /// </summary>
        PoolLevels,
        /// <summary>
        /// Staging values were mixed into the pool.
        /// </summary>
        MixStagingPool,
        /// <summary>
        /// A clean pool operation was completed.
        /// </summary>
        CleanPool
    }
}