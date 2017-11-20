using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Crypto.KAS.Scheme
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