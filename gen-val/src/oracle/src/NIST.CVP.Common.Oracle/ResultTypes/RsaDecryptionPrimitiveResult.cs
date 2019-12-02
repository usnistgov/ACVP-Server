using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaDecryptionPrimitiveResult
    {
        public BitString CipherText { get; set; }
        public KeyPair Key { get; set; }
        public BitString PlainText { get; set; }
        public bool TestPassed { get; set; }
    }
}
