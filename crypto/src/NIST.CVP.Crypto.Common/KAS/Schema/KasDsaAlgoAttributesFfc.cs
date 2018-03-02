using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Schema
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