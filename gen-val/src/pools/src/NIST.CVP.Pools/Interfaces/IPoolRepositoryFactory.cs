using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools.Interfaces
{
    /// <summary>
    /// Interface for retrieving a <see cref="IPoolRepository{TResult}"/>
    /// </summary>
    public interface IPoolRepositoryFactory
    {
        /// <summary>
        /// Get a <see cref="IPoolRepository{TResult}"/> supporting the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type of result supported by the repository.</typeparam>
        /// <returns>A repository supporting the specified type.</returns>
        IPoolRepository<TResult> GetRepository<TResult>() where TResult : IResult;
    }
}