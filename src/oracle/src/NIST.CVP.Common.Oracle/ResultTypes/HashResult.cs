using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class HashResult
    {
        public BitString Message { get; set; }
        public BitString Digest { get; set; }
    }
}
