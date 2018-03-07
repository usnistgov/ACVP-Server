using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core
{
    public class TestCaseValidation
    {
        [JsonProperty(PropertyName = "tcId")]
        public int TestCaseId { get; set; }
        public Disposition Result { get; set; }
        public string Reason { get; set; }
    }
}
