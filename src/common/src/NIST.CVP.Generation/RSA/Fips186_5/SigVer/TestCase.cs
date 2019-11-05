using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA.Fips186_5.SigVer.TestCaseExpectations;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
        public bool Deferred { get; set; }

        public BitString Message { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString RandomValue { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RandomValueLen { get; set; }

        public BitString Signature { get; set; }
        public BitString Salt { get; set; }

        [JsonIgnore]
        public ITestCaseExpectationReason<SignatureModifications> Reason { get; set; }      // Tells us what value was modified leading to the failure

        [JsonProperty(PropertyName = "reason")]
        public string ReasonName
        {
            get => Reason.GetName();
            set => Reason = new TestCaseExpectationReason(EnumHelpers.GetEnumFromEnumDescription<SignatureModifications>(value));
        }
    }
}