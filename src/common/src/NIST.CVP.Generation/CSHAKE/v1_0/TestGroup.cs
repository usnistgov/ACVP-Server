using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CSHAKE.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public string Function { get; set; }

        [JsonIgnore]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "hexCustomization")]
        public bool HexCustomization { get; set; } = false;

        [JsonIgnore]
        public MathDomain OutputLength { get; set; }

        [JsonIgnore]
        public MathDomain MessageLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
