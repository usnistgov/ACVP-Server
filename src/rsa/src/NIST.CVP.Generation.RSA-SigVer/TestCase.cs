using Newtonsoft.Json;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.RSA_SigVer.TestCaseExpectations;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString Message { get; set; }
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
