using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC.AES
{
    public class TestCase : TestCaseBase
    {
        public TestCase()
        {

        }
        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public override bool Merge(ITestCase promptTestCase)
        {
            if (TestCaseId == promptTestCase.TestCaseId)
            {
                return true;
            }

            return false;
        }

        public override bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "key":
                    Key = new BitString(value);
                    return true;
                case "msg":
                    Message = new BitString(value);
                    return true;
                case "mac":
                    Mac = new BitString(value);
                    return true;
            }
            return false;
        }

        protected override void MapToProperties(dynamic source)
        {
            TestCaseId = (int)source.tcId;

            ExpandoObject expandoSource = (ExpandoObject)source;

            if (((ExpandoObject)source).ContainsProperty("failureTest"))
            {
                FailureTest = source.failureTest;
            }
            if (((ExpandoObject)source).ContainsProperty("result"))
            {
                Result = source.result;
            }

            Key = expandoSource.GetBitStringFromProperty("key");
            Message = expandoSource.GetBitStringFromProperty("msg");
            Mac = expandoSource.GetBitStringFromProperty("mac");
        }
    }
}
