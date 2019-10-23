using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class RsaSignatureParameters
    {
        public SignatureModifications Reason { get; set; }
        public int Modulo { get; set; }
        public HashFunction HashAlg { get; set; }
        public int SaltLength { get; set; }
        public SignatureSchemes PaddingScheme { get; set; }
        public KeyPair Key { get; set; }
        public PssMaskTypes MaskFunction { get; set; }
        public bool IsMessageRandomized { get; set; }
    }
}
