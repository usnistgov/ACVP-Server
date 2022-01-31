using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class LmsKeyResult
    {
        public BitString SEED { get; set; }
        public BitString I { get; set; }
        public HssKeyPair KeyPair { get; set; }
    }
}
