using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaSignatureResult
    {
        public BitString Message { get; set; }
        public BitString RandomValue { get; set; }
        public KeyPair Key { get; set; }
        public BitString Salt { get; set; }

        public BitString Signature { get; set; }
    }
}
