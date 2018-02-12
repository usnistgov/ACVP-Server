using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using System.Dynamic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.RSA_SigVer.TestCaseExpectations;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public BitString Message { get; set; }
        public BitString Signature { get; set; }
        public BitString Salt { get; set; }
        public bool Result { get; set; }
        public ITestCaseExpectationReason<SignatureModifications> Reason { get; set; }      // Tells us what value was modified leading to the failure

        public TestCase() { }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        private void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            var expandoSource = (ExpandoObject) source;

            Message = expandoSource.GetBitStringFromProperty("message");
            Signature = expandoSource.GetBitStringFromProperty("signature");
            Salt = expandoSource.GetBitStringFromProperty("salt");
            Result = expandoSource.GetTypeFromProperty<bool>("result");

            var reasonValue = expandoSource.GetTypeFromProperty<string>("reason");
            var reasonEnum = EnumHelpers.GetEnumFromEnumDescription<SignatureModifications>(reasonValue, false);
            Reason = new TestCaseExpectationReason(reasonEnum);
        }

        public bool Merge(ITestCase otherTest)
        {
            if(TestCaseId != otherTest.TestCaseId)
            {
                return false;
            }

            var otherTypedTest = (TestCase)otherTest;
            if(Salt == null & otherTypedTest.Salt != null)
            {
                Salt = otherTypedTest.Salt.GetDeepCopy();
            }

            if(Message == null && otherTypedTest.Message != null)
            {
                Message = otherTypedTest.Message.GetDeepCopy();
                Signature = otherTypedTest.Signature.GetDeepCopy();
                return true;
            }

            return false;
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
                    Result = (value[0] == 'p');
                    return true;

                case "saltval":
                    Salt = new BitString(value);
                    return true;
            }

            return false;
        }
    }
}
