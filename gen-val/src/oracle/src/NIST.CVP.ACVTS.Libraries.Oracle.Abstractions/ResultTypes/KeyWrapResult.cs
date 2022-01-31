using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class KeyWrapResult
    {
        public BitString Key { get; set; }
        public BitString Plaintext { get; set; }
        public BitString Ciphertext { get; set; }
        public bool TestPassed { get; set; }
    }
}
