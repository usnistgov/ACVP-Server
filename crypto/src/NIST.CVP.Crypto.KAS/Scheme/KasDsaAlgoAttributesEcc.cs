using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Crypto.KAS.Scheme
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