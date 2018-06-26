namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class VerifyResult<T>
    {
        public T Signature { get; set; }
        public bool TestPassed { get; set; }
    }
}
