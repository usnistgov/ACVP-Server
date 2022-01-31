using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasAftParametersEcc : KasAftParametersBase
    {
        public Curve Curve { get; set; }

        public EccScheme EccScheme { get; set; }

        public EccParameterSet EccParameterSet { get; set; }
    }
}
