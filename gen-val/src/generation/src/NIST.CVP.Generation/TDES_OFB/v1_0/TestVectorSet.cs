using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFB.v1_0
{
    public class TestVectorSet : ITestVectorSet<TestGroup, TestCase>
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; } = "TDES-OFB";

        [JsonIgnore]
        public string Mode { get; set; } = string.Empty;
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public List<TestGroup> TestGroups { get; set; } = new List<TestGroup>();
    }
}
