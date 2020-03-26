using NIST.CVP.Crypto.Common.KAS;

namespace NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2
{
    public class KasAftDeferredResult
    {
        public IIfcSecretKeyingMaterial ServerKeyingMaterial { get; set; }
        public IIfcSecretKeyingMaterial IutKeyingMaterial { get; set; }
        public KasResult Result { get; set; }
    }
}