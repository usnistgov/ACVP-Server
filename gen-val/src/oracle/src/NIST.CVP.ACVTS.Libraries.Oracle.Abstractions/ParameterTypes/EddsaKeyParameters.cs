using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class EddsaKeyParameters
    {
        public Curve Curve { get; set; }
        public EddsaKeyDisposition Disposition { get; set; }
    }
}
