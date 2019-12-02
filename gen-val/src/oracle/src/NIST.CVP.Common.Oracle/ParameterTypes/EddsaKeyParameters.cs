using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class EddsaKeyParameters
    {
        public Curve Curve { get; set; }
        public EddsaKeyDisposition Disposition { get; set; }
    }
}
