using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Scheme
{
    public class KasAlgoAttributesIfc : IKasAlgoAttributes
    {
        public KasAlgoAttributesIfc(IfcScheme scheme)
        {
            Scheme = scheme;
        }

        public IfcScheme Scheme { get; }
    }
}