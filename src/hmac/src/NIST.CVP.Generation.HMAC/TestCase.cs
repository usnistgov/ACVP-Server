using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.HMAC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "msg")]
        public BitString Message { get; set; }
        public BitString Mac { get; set; }

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
    }
}
