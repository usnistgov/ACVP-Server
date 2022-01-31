using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer
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

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "msg":
                    Message = new BitString(value);
                    return true;

                case "s":
                    Signature = new BitString(value);
                    return true;

                case "result":
                    TestPassed = (value[0] == 'p');
                    return true;

                case "saltval":
                    Salt = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
