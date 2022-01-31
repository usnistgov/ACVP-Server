using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
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
