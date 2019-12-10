using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Scheme;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3
{
    public class KasValResult
    {
        public ISecretKeyingMaterial ServerSecretKeyingMaterial { get; set; }
        public ISecretKeyingMaterial IutSecretKeyingMaterial { get; set; }
        
        public KasValTestDisposition Disposition { get; set; }
        public bool TestPassed { get; set; }
    
        public IKdfParameter KdfParameter { get; set; }
        
        public MacParameters MacParameters { get; set; }
        
        public KeyAgreementResult KasResult { get; set; }
    }
}