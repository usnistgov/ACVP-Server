using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3
{
    public class KasSscAftDeferredResult
    {
        public ISecretKeyingMaterial ServerSecretKeyingMaterial { get; set; }
        public ISecretKeyingMaterial IutSecretKeyingMaterial { get; set; }
        public KeyAgreementResult KasResult { get; set; }
    }
}
