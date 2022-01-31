using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3
{
    public class KasAftResult
    {
        public ISecretKeyingMaterial ServerSecretKeyingMaterial { get; set; }
        public IKdfParameter KdfParameter { get; set; }
    }
}
