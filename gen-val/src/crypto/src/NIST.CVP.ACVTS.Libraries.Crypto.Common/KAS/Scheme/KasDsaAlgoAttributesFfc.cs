using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
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
