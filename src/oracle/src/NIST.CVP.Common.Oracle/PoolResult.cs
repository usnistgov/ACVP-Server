namespace NIST.CVP.Common.Oracle
{
    public class PoolResult<TResult>
    {
        public bool PoolEmpty { get; set; }
        public TResult Result { get; set; }
    }
}
