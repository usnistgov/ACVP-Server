using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class RsaSignaturePrimitiveResult
    {
        public KeyPair Key { get; set; }
        public BitString Message { get; set; }
        public BitString Signature { get; set; }
        public bool ShouldPass { get; set; }
    }
}
