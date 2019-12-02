using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools.Models
{
    public class PoolResult<TResult>
        where TResult : IResult
    {
        public bool PoolTooEmpty { get; set; }
        public TResult Result { get; set; }
    }
}
