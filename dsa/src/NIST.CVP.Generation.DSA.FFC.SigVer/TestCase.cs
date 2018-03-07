using System.Dynamic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred { get; set; }

        public bool Result { get; set; }
        public ITestCaseExpectationReason<SigFailureReasons> Reason { get; set; }
        public FfcKeyPair Key { get; set; }
        public BitString Message { get; set; }
        public FfcSignature Signature { get; set; }

        // Needed for FireHoseTests
        private BigInteger _rSetString;
        private BigInteger _sSetString;

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

            var expandoSource = (ExpandoObject)source;

            var resultValue = expandoSource.GetTypeFromProperty<string>("result");
            if (resultValue != null)
            {
                Result = resultValue.ToLower() == "passed";
            }

            var reasonValue = expandoSource.GetTypeFromProperty<string>("reason");
            if (reasonValue != null)
            {
                Reason = new TestCaseExpectationReason(EnumHelpers.GetEnumFromEnumDescription<SigFailureReasons>(expandoSource.GetTypeFromProperty<string>("reason")));
            }
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "y":
                    Key = new FfcKeyPair(new BitString(value).ToPositiveBigInteger());
                    return true;

                case "msg":
                    Message = new BitString(value);
                    return true;

                case "r":
                    _rSetString = new BitString(value).ToPositiveBigInteger();
                    return true;

                case "s":
                    _sSetString = new BitString(value).ToPositiveBigInteger();
                    Signature = new FfcSignature(_rSetString, _sSetString);
                    return true;

                case "result":
                    FailureTest = value.ToLower()[0] == 'f';
                    return true;
            }

            return false;
        }
    }
}
