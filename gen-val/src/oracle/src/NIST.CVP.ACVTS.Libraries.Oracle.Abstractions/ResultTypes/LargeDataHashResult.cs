using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class LargeDataHashResult
    {
        public LargeBitString MessageContent { get; set; }
        public BitString Digest { get; set; }
    }
}
