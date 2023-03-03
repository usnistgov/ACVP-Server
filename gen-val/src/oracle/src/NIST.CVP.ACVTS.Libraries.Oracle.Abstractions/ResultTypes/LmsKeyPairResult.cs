using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class LmsKeyPairResult : IResult
    {
        public BitString Seed { get; set; }
        public BitString I { get; set; }
        public ILmsKeyPair KeyPair { get; set; }
    }
}
