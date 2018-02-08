using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(dynamic source)
        {
            TestGroupId = source.tgId;
            AADLength = source.aadLen;
            PTLength = source.ptLen;
            IVLength = source.ivLen;
            TagLength = source.tagLen;
            KeyLength = source.keyLen;
            Function = source.direction;
            TestType = source.testType;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }

        }

        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keylen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "ivlen")]
        public int IVLength { get; set; }
        [JsonProperty(PropertyName = "ptlen")]
        public int PTLength { get; set; }
        [JsonProperty(PropertyName = "aadlen")]
        public int AADLength { get; set; }
        [JsonProperty(PropertyName = "taglen")]
        public int TagLength { get; set; }
        public List<ITestCase> Tests { get; set; }

        public bool GroupReusesKeyForTestCases { get; set; }
        public bool GroupReusesNonceForTestCases { get; set; }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            if (!int.TryParse(value, out int intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = intVal;
                    return true;
                case "aadlen":
                    AADLength = intVal;
                    return true;
                case "taglen":
                    TagLength = intVal;
                    return true;
                case "ivlen":
                    IVLength = intVal;
                    return true;
                case "ptlen":
                    PTLength = intVal;
                    return true;
            }
            return false;
        }
    }
}
