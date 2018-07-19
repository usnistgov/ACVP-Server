using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed => true;
        public bool Deferred => false;
        public TestGroup ParentGroup { get; set; }
        public BitString Key { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength
        {
            get
            {
                if (Key == null)
                {
                    return 0;
                }
                return Key.BitLength;
            }
        }
        [JsonProperty(PropertyName = "msg")]
        public BitString Message { get; set; }
        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength
        {
            get
            {
                if (Message == null)
                {
                    return 0;
                }
                return Message.BitLength;
            }
        }
        public BitString Mac { get; set; }
        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }
        public bool? MacVerified { get; set; }

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; }

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
