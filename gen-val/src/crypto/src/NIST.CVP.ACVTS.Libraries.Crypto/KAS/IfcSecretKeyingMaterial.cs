using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS
{
    internal class IfcSecretKeyingMaterial : IIfcSecretKeyingMaterial
    {
        public KeyPair Key { get; set; }
        public BitString Z { get; set; }
        public BitString DkmNonce { get; set; }
        public BitString PartyId { get; set; }
        public BitString C { get; set; }
        public BitString K { get; set; }
    }
}
