using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Scheme;

namespace NIST.CVP.Crypto.KAS
{
    public class KasIfc : IKasIfc
    {
        public ISchemeIfc Scheme { get; }
        public KasResult ComputeResult(IIfcSecretKeyingMaterial otherPartyKeyingMaterial)
        {
            throw new System.NotImplementedException();
        }
    }
}