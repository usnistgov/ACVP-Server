namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class VerifyResult<T>
    {
        public T VerifiedValue { get; set; }
        public bool Result { get; set; }
    }
}
