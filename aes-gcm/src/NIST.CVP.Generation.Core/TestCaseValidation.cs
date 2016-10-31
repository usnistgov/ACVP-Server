using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NIST.CVP.Generation.Core
{
    public class TestCaseValidation
    {
        [JsonProperty(PropertyName = "tcId")]
        public int TestCaseId { get; set; }
        public string Result { get; set; }
        public string Reason { get; set; }
    }
}
