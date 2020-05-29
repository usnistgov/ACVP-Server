using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2
{
    public class KasAftResultIfc
    {
        public KeyPair ServerKeyPair { get; set; }
        public BitString ServerNonce { get; set; }
        public BitString ServerC { get; set; }
        public BitString ServerZ { get; set; }
        public BitString ServerK { get; set; }
        
        
        public KeyPair IutKeyPair { get; set; }

        public IKdfParameter KdfParameter { get; set; }
        public KtsParameter KtsParameter { get; set; }
    }
}