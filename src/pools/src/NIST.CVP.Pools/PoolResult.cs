using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Pools
{
    public class PoolResult<TResult>
        where TResult : IResult
    {
        public bool PoolEmpty { get; set; }
        public int TimesValueUsed {get; set; }
        public TResult Result { get; set; }
    }
}
