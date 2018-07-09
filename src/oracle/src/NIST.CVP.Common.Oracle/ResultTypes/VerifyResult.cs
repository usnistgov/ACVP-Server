namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class VerifyResult<T>
    {
        public T Signature { get; set; }
        public bool Result { get; set; }
    }
}
