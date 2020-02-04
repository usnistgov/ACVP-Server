using System.Collections.Generic;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Interfaces
{
    /// <summary>
    /// Describes operations that manage pool values
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IPoolRepository<TResult>
        where TResult : IResult
    {
        /// <summary>
        /// Gets a count of the number of values residing in a pool.
        /// </summary>
        /// <param name="poolName">The pool in which to count values.</param>
        /// <param name="useStagingPool">Flag for checking the staging vs actual pool levels.</param>
        /// <returns>Number of values in a pool.</returns>
        long GetPoolCount(string poolName, bool useStagingPool);

        /// <summary>
        /// Get a result from the specified pool
        /// </summary>
        /// <param name="poolName">The pool to pull a value from.</param>
        /// <returns>A pool result</returns>
        PoolObject<TResult> GetResultFromPool(string poolName);

        /// <summary>
        /// Adds a result to the specified pool
        /// </summary>
        /// <param name="poolName">The pool to add a value to.</param>
        /// <param name="useStagingPool">Should the value be added to the staging pool or normal pool?</param>
        /// <param name="value">The value to add.</param>
        void AddResultToPool(string poolName, bool useStagingPool, PoolObject<TResult> value);

        /// <summary>
        /// Mix the current staging values into the base pool
        /// </summary>
        /// <param name="poolName">The normal pool.</param>
        void MixStagingPoolIntoPool(string poolName);

        /// <summary>
        /// Clean the provided pools values.
        /// </summary>
        /// <param name="poolName">The pool to clean.</param>
        void CleanPool(string poolName);

        /// <summary>
        /// Retrieves pool levels for all pools - used in initial pool manager construction.
        /// </summary>
        /// <returns>Dictionary of poolName, poolCount</returns>
        Dictionary<string, long> GetAllPoolCounts();
    }
}