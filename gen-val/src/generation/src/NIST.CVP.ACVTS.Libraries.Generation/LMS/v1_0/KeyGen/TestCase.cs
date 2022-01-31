using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        [JsonIgnore]
        public bool? TestPassed => true;
        public bool Deferred => true;
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "pub")]
        public BitString PublicKey { get; set; }
        [JsonProperty(PropertyName = "seed")]
        public BitString Seed { get; set; }
        [JsonProperty(PropertyName = "rootI")]
        public BitString RootI { get; set; }
    }
}
