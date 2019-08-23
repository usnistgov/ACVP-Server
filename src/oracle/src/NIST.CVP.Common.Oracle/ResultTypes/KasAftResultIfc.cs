using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class KasAftResultIfc
    {
        public KeyPair ServerKeyPair { get; set; }
        public BitString ServerNonce { get; set; }
        public BitString ServerC { get; set; }
        public BitString ServerZ { get; set; }
        
        #region Sample only properties
        public KeyPair IutKeyPair { get; set; }
        public BitString IutNonce { get; set; }
        public BitString IutC { get; set; }
        public BitString IutZ { get; set; }
        
        public IKdfParameter KdfParameter { get; set; }
        public Crypto.Common.KAS.KDF.KdfResult KdfResult { get; set; }
        
        public KtsParameter KtsParameter { get; set; }
        public SharedSecretResponse KtsResult { get; set; }
        
        public MacParameters MacParameters { get; set; }
        
        public KasResult KasResult { get; set; }
        
        #endregion Sample only properties
    }
}