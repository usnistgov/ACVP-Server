using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "pt")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "payloadLen")]
        public int PayloadLength { get; set; }
        [JsonProperty(PropertyName = "ct", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString CipherText { get; set; }
        [JsonProperty(PropertyName = "iv", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BitString IV { get; set; }
        [JsonProperty(PropertyName = "key")]
        public BitString Key { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "pt":
                case "plaintext":
                case "p":
                    PlainText = new BitString(value);
                    return true;

                case "ct":
                case "ciphertext":
                case "c":
                    CipherText = new BitString(value);
                    return true;

                case "iv":
                    IV = new BitString(value);
                    return true;

                case "key":
                    Key = new BitString(value);
                    return true;
            }
            return false;
        }
    }
}
