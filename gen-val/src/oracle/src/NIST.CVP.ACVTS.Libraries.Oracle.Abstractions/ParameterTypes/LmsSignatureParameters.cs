using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class LmsSignatureParameters
    {
        public int Layers { get; set; }
        public LmsType[] LmsTypes { get; set; }
        public LmotsType[] LmotsTypes { get; set; }
        public int Advance { get; set; }
        public LmsSignatureDisposition Disposition { get; set; }
        public bool IsSample { get; set; }
    }
}
