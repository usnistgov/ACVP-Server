using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class RsaDecryptionPrimitiveResult : ICryptoResult
    {
        public BitString CipherText { get; set; }
        public KeyPair Key { get; set; }
        public BitString PlainText { get; set; }
        public bool TestPassed { get; set; }
        public string ErrorMessage { get; set; }
    }
}
