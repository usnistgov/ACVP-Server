using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools
{
    public interface IPool<out TParam, TResult> : IPool
        where TParam : IParameters
        where TResult : IResult
    {
        TParam WaterType { get; }

        bool AddWater(TResult value);
        PoolResult<TResult> GetNext();
    }

    public interface IPool
    {
        bool IsEmpty { get; }
        int WaterLevel { get; }
        
        Type ParamType { get; }
        IParameters Param { get; }
        Type ResultType { get; }
        bool AddWater(IResult value);

        PoolResult<IResult> GetNextUntyped();

        bool SavePoolToFile(string filename);
    }
}