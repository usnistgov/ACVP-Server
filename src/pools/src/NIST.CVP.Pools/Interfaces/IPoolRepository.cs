using NIST.CVP.Common.Oracle.ResultTypes;

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
        /// <returns>Number of values in a pool.</returns>
        long GetPoolCount(string poolName);

        /// <summary>
        /// Get a result from the specified pool
        /// </summary>
        /// <param name="poolName">The pool to pull a value from.</param>
        /// <returns>A pool result</returns>
        TResult GetResultFromPool(string poolName);

        /// <summary>
        /// Adds a result to the specified pool
        /// </summary>
        /// <param name="poolName">The pool to add a value to.</param>
        /// <param name="value">The value to add.</param>
        void AddResultToPool(string poolName, TResult value);

        /// <summary>
        /// Mix the current staging values into the base pool
        /// </summary>
        /// <param name="stagingPoolName">The staging pool.</param>
        /// <param name="poolName">The normal pool.</param>
        void MixStagingPoolIntoPool(string stagingPoolName, string poolName);

        /// <summary>
        /// Clean the provided pools values.
        /// </summary>
        /// <param name="poolName">The pool to clean.</param>
        void CleanPool(string poolName);
    }
}