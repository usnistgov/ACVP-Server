using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class HashResult : Result
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
    }
}
