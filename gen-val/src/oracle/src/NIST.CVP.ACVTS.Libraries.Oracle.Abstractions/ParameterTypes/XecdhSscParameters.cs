using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class XecdhSscParameters
    {
        public Curve Curve { get; set; }

        public bool IsSample { get; set; }
    }
}
