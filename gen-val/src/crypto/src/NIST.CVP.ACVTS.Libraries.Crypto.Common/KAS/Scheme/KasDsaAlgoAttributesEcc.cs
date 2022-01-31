using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme
{
    public class KasDsaAlgoAttributesEcc : IKasAlgoAttributes
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
