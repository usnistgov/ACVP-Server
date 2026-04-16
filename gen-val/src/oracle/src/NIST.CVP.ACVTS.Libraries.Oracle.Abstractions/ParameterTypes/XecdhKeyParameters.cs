using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class XecdhKeyParameters
    {
        public Curve Curve { get; set; }
        public XecdhKeyDisposition Disposition { get; set; }
    }
}
