using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Sp800_56Ar3
{
    internal class Kas : IKas
    {
        public IScheme Scheme { get; }

        public Kas(IScheme scheme)
        {
            Scheme = scheme;
        }

        public KeyAgreementResult ComputeResult(ISecretKeyingMaterial otherPartyKeyingMaterial)
        {
            return Scheme.ComputeResult(otherPartyKeyingMaterial);
        }
    }
}
