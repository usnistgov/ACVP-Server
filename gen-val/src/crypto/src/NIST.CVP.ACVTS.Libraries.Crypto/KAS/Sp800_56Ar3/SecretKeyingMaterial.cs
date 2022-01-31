using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3
{
    internal class SecretKeyingMaterial : ISecretKeyingMaterial
    {
        public KasAlgorithm KasAlgorithm { get; set; }
        public IDsaDomainParameters DomainParameters { get; set; }
        public IDsaKeyPair StaticKeyPair { get; set; }
        public IDsaKeyPair EphemeralKeyPair { get; set; }
        public BitString EphemeralNonce { get; set; }
        public BitString DkmNonce { get; set; }
        public BitString PartyId { get; set; }
    }
}
