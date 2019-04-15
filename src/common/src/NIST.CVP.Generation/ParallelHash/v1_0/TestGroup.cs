using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ParallelHash.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public string Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }
        
        [JsonProperty(PropertyName = "XOF")]
        public bool XOF { get; set; }

        [JsonIgnore]
        public bool HexCustomization { get; set; } = false;

        [JsonIgnore]
        public MathDomain OutputLength { get; set; }

        [JsonIgnore]
        public MathDomain MessageLength { get; set; }
    }
}
