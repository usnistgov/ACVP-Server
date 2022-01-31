using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class HashResult : IResult
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
    }
}
