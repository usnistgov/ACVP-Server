using NIST.CVP.Common.Enums;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.GenValApp.Models
{
    public class Error
    {
        public StatusCode StatusCode { get; set; }
        public string Source { get; set; }
        public string Message => EnumHelpers.GetEnumDescriptionFromEnum(StatusCode);
        public string AdditionalInformation { get; set; }
    }
}
