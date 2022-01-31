using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3
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
