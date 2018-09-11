using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class EcdsaKeyParameters : IParameters
    {
        public Curve Curve { get; set; }
        public EcdsaKeyDisposition Disposition { get; set; }
    }
}
