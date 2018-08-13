using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaSignaturePrimitiveResult
    {
        public KeyPair Key { get; set; }
        public BitString Message { get; set; }
        public BitString Signature { get; set; }
        public bool ShouldPass { get; set; }
    }
}
