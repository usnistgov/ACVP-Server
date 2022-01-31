using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2
{
    public class KasAftDeferredResult
    {
        public IIfcSecretKeyingMaterial ServerKeyingMaterial { get; set; }
        public IIfcSecretKeyingMaterial IutKeyingMaterial { get; set; }
        public KasResult Result { get; set; }
    }
}
