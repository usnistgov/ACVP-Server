using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.Interfaces
{
    /// <summary>
    /// Provides a way to wrap an object in a Mongo friendly object.
    /// </summary>
    public interface IMongoPoolObjectFactory
    {
        /// <summary>
        /// Wraps the provided type in a Mongo friendly object.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        MongoPoolObject<TResult> WrapResult<TResult>(TResult result) where TResult : IResult;
    }
}