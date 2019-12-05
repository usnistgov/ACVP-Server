using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;
using System;

namespace NIST.CVP.Pools.Interfaces
{
    /// <summary>
    /// Provides a way to wrap an object in a Mongo friendly object.
    /// </summary>
    public interface IPoolObjectFactory
    {
        /// <summary>
        /// Wraps the provided type in a storage friendly object.
        /// </summary>
        /// <typeparam name="TResult">Pool value type to wrap.</typeparam>
        /// <param name="result">The pool value to wrap.</param>
        /// <returns>The wrapped pool value.</returns>
        PoolObject<TResult> WrapResult<TResult>(TResult result) where TResult : IResult;

        /// <summary>
        /// Wrap an already used value for reintroduction into the pool.
        /// </summary>
        /// <typeparam name="TResult">Pool value type to wrap.</typeparam>
        /// <param name="result">The pool value to wrap.</param>
        /// <param name="creationDate">Value creation date.</param>
        /// <param name="lastUsedDate">Last time the value was used.</param>
        /// <param name="timesUsed">Number of times the value ha been used.</param>
        /// <returns></returns>
        PoolObject<TResult> WrapResult<TResult>(
            TResult result, DateTime creationDate, DateTime lastUsedDate, int timesUsed
        ) where TResult : IResult;
    }
}