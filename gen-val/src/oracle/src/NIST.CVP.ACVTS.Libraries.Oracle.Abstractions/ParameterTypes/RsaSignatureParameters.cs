using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
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
        public int MessageLength { get; set; }
    }
}
