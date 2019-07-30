using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Scheme
{
    public class KasDsaAlgoAttributesFfc : IKasAlgoAttributes
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