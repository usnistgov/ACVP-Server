using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3;

namespace NIST.CVP.Crypto.KAS.Sp800_56Ar3
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