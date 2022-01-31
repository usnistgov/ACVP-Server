using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        [JsonIgnore]
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

        [JsonProperty(PropertyName = "customization")]
        public string Customization { get; set; }

        [JsonProperty(PropertyName = "customizationHex")]
        public BitString CustomizationHex { get; set; }

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
