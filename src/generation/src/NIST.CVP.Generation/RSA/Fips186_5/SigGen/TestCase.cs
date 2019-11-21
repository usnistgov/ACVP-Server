using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore]
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString RandomValue { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RandomValueLen { get; set; }

        public BitString Signature { get; set; }
        public BitString Salt { get; set; }
    }
}