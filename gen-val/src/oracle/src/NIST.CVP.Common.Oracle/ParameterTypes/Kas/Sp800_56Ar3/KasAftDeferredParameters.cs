using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3
{
    public class KasAftDeferredParameters : KasParametersBase
    {
        public ISecretKeyingMaterial ServerSecretKeyingMaterial { get; set; }
        public ISecretKeyingMaterial IutSecretKeyingMaterial { get; set; }
        
        public int L { get; set; }
        
        public IKdfConfiguration KdfConfiguration { get; set; }
        public MacConfiguration MacConfiguration { get; set; }
    }
}