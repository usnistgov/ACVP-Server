using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System;

namespace NIST.CVP.Pools
{
    public interface IPool<out TParam, TResult> : IPool
        where TParam : IParameters
        where TResult : IResult
    {
        /// <summary>
        /// The type of parameter this pool can support
        /// </summary>
        TParam WaterType { get; }

        /// <summary>
        /// Adds a precomputed value to the pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool AddWater(TResult value);

        /// <summary>
        /// Gets a value from the pool
        /// </summary>
        /// <returns></returns>
        PoolResult<TResult> GetNext();
    }

    public interface IPool
    {
        /// <summary>
        /// Is the pool empty?
        /// </summary>
        bool IsEmpty { get; }
        
        /// <summary>
        /// The number of values within the pool
        /// </summary>
        int WaterLevel { get; }
        
        /// <summary>
        /// The type of parameter class this pool supports
        /// </summary>
        Type ParamType { get; }
        
        /// <summary>
        /// The enum type pertaining to this pool
        /// </summary>
        PoolTypes DeclaredType { get; }
        
        /// <summary>
        /// The Parameter instance
        /// </summary>
        IParameters Param { get; }
        
        /// <summary>
        /// The type of result this pool returns
        /// </summary>
        Type ResultType { get; }
        
        /// <summary>
        /// Add a value to the pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool AddWater(IResult value);

        /// <summary>
        /// Get a result from the pool
        /// </summary>
        /// <returns></returns>
        PoolResult<IResult> GetNextUntyped();

        /// <summary>
        /// Save the pool to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        bool SavePoolToFile(string filename);

        /// <summary>
        /// Clears the pool of all values
        /// </summary>
        /// <returns></returns>
        bool CleanPool();
    }
}