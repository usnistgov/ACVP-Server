using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class Blake2Result : IResult
    {
        public BitString Message { get; set; }
        public BitString Key { get; set; }
        public BitString Digest { get; set; }
    }
}
