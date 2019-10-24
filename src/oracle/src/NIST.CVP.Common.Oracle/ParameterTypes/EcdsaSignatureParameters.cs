using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class EcdsaSignatureParameters
    {
        public bool PreHashedMessage { get; set; }
        public EccKeyPair Key { get; set; }
        public HashFunction HashAlg { get; set; }
        public Curve Curve { get; set; }
        public EcdsaSignatureDisposition Disposition { get; set; }
        public NonceProviderTypes NonceProviderType { get; set; }
        public bool IsMessageRandomized { get; set; }
    }
}
