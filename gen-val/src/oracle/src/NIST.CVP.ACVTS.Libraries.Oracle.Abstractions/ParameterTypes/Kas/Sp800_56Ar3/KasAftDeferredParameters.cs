using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasAftDeferredParameters : KasParametersBase
    {
        public BitString DkmNonceServer { get; set; }
        public BitString EphemeralNonceServer { get; set; }
        public IDsaKeyPair EphemeralKeyServer { get; set; }
        public IDsaKeyPair StaticKeyServer { get; set; }

        public BitString PartyIdIut { get; set; }
        public BitString DkmNonceIut { get; set; }
        public BitString EphemeralNonceIut { get; set; }
        public IDsaKeyPair EphemeralKeyIut { get; set; }
        public IDsaKeyPair StaticKeyIut { get; set; }

        public int L { get; set; }

        public IKdfParameter KdfParameter { get; set; }
        public MacParameters MacParameter { get; set; }
    }
}
