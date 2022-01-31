using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.CBC_MAC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }

        [JsonIgnore]
        public TestGroup ParentGroup { get; set; }

        [JsonIgnore]
        public bool? TestPassed { get; set; }

        [JsonIgnore]
        public bool Deferred { get; set; }

        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }

        public BitString Key { get; set; }

        [JsonProperty(PropertyName = "ct")]
        public BitString CipherText { get; set; }
    }
}
