using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Schema
{
    public class KasDsaAlgoAttributesEcc : IKasDsaAlgoAttributes
    {
        public KasDsaAlgoAttributesEcc(EccScheme scheme, EccParameterSet parameterSet, Curve curveName)
        {
            Scheme = scheme;
            ParameterSet = parameterSet;
            CurveName = curveName;
        }

        public EccScheme Scheme { get; }
        public EccParameterSet ParameterSet { get; }
        public Curve CurveName { get; }
    }
}