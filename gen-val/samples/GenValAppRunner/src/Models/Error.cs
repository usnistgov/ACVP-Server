using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;

namespace NIST.CVP.ACVTS.Generation.GenValApp.Models
{
    public class Error
    {
        public StatusCode StatusCode { get; set; }
        public string Source { get; set; }
        public string Message => EnumHelpers.GetEnumDescriptionFromEnum(StatusCode);
        public string AdditionalInformation { get; set; }
    }
}
