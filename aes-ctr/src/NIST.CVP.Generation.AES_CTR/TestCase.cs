using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }
        public TestGroup ParentGroup { get; set; }

        [JsonProperty(PropertyName = "plainText")]
        public BitString PlainText { get; set; }
        [JsonProperty(PropertyName = "dataLength")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "cipherText")]
        public BitString CipherText { get; set; }
        [JsonProperty(PropertyName = "iv")]
        public BitString IV { get; set; }
        [JsonProperty(PropertyName = "ivs")]
        public List<BitString> IVs { get; set; }
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
