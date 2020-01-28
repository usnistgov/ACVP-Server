using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1
{
    public class KasEccComponentParameters
    {
        public Curve Curve { get; set; }
        
        public bool IsSample { get; set; }
    }
}