using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    internal class KasIfc : IKasIfc
    {
        public KasIfc(ISchemeIfc scheme)
        {
            Scheme = scheme;
        }

        public ISchemeIfc Scheme { get; }

        public void InitializeThisPartyKeyingMaterial(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            Scheme.InitializeThisPartyKeyingMaterial(otherPartyKeyingMaterial);
        }

        public KasIfcResult ComputeResult(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            return Scheme.ComputeResult(otherPartyKeyingMaterial);
        }
    }
}