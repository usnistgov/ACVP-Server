using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class EcdsaSignatureResult
    {
        public BitString Message { get; set; }
        public EccKeyPair Key { get; set; }
        public EccSignature Signature { get; set; }
    }
}
