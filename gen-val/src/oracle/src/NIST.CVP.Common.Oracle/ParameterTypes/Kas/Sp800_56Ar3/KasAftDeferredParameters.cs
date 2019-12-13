using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasAftDeferredParameters : KasParametersBase
    {
        public BitString ServerDkmNonce { get; set; }
        public BitString ServerEphemeralNonce { get; set; }
        public IDsaKeyPair ServerEphemeralKey { get; set; }
        public IDsaKeyPair ServerStaticKey { get; set; }
        
        public BitString IutPartyId { get; set; }
        public BitString IutDkmNonce { get; set; }
        public BitString IutEphemeralNonce { get; set; }
        public IDsaKeyPair IutEphemeralKey { get; set; }
        public IDsaKeyPair IutStaticKey { get; set; }
        
        public int L { get; set; }
        
        public IKdfParameter KdfParameter { get; set; }
        public MacParameters MacParameter { get; set; }
    }
}