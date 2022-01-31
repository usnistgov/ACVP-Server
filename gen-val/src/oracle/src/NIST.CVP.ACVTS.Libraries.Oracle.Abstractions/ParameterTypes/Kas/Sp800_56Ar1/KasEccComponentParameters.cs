using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasEccComponentParameters
    {
        public Curve Curve { get; set; }

        public bool IsSample { get; set; }
    }
}
