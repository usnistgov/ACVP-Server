using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;

namespace NIST.CVP.Crypto.KAS.Scheme.Ffc
{
    public class KasDsaAlgoAttributesFfc : IKasDsaAlgoAttributes
    {
        public KasDsaAlgoAttributesFfc(FfcScheme scheme, FfcParameterSet parameterSet)
        {
            Scheme = scheme;
            ParameterSet = parameterSet;
        }

        public FfcScheme Scheme { get; }
        public FfcParameterSet ParameterSet { get; }
    }
}