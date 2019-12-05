using NIST.CVP.Crypto.Common.KAS;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class KasAftDeferredResult
    {
        public IIfcSecretKeyingMaterial ServerKeyingMaterial { get; set; }
        public IIfcSecretKeyingMaterial IutKeyingMaterial { get; set; }
        public KasResult Result { get; set; }
    }
}
