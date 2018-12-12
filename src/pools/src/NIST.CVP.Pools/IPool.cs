using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using NIST.CVP.Pools.Models;
using System;
using System.Threading.Tasks;

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
        /// The maximum amount of water allowed in a pool.
        /// </summary>
        int MaxWaterLevel { get; }

        /// <summary>
        /// The minimum amount of water allowed in a pool for the pool to be used
        /// </summary>
        int MinWaterLevel { get; }

        /// <summary>
        /// Probability a value will be reused
        /// </summary>
        decimal RecycleRate { get; }

        decimal WaterFillPercent { get; }

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
        /// Submit a request to fill the pool with water
        /// </summary>
        /// <returns></returns>
        Task RequestWater();

        /// <summary>
        /// Get a result from the pool
        /// </summary>
        /// <returns></returns>
        PoolResult<IResult> GetNextUntyped();

        /// <summary>
        /// Save the pool to a file
        /// </summary>
        /// <returns></returns>
        bool SavePoolToFile();

        /// <summary>
        /// Clears the pool of all values
        /// </summary>
        /// <returns></returns>
        bool CleanPool();
    }
}