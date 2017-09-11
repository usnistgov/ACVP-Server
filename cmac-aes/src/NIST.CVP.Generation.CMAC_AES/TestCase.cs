using System.Dynamic;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestCase : ITestCase
    {

        public int TestCaseId { get; set; }
        public bool FailureTest { get; set; }
        public bool Deferred => false;
        public BitString Key { get; set; }
        public BitString Message { get; set; }
        public BitString Mac { get; set; }
        public string Result { get; set; }

        public TestCase() { }

        public TestCase(dynamic source)
        {
            MapToProperties(source);
        }

        public TestCase(JObject source)
        {
            var data = source.ToObject<ExpandoObject>();
            MapToProperties(data);
        }

        public bool Merge(ITestCase promptTestCase)
        {
            if (TestCaseId == promptTestCase.TestCaseId)
            {
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

        private void MapToProperties(dynamic source)
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
