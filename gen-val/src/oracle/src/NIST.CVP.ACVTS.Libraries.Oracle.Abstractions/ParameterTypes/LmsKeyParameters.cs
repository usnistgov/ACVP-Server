using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class LmsKeyParameters
    {
        public int Layers { get; set; }
        public LmsType[] LmsTypes { get; set; }
        public LmotsType[] LmotsTypes { get; set; }
    }
}
