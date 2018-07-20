using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class KeyWrapResult
    {
        public BitString Key { get; set; }
        public BitString Plaintext { get; set; }
        public BitString Ciphertext { get;set; }
        public bool TestPassed { get; set; }
    }
}