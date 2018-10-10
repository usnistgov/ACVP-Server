using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "ivlen")]
        public int IVLength { get; set; }
        [JsonProperty(PropertyName = "ptlen")]
        public int PTLength { get; set; }
        [JsonProperty(PropertyName = "aadlen")]
        public int AADLength { get; set; }
        [JsonProperty(PropertyName = "taglen")]
        public int TagLength { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        [JsonIgnore]
        public int[] AADLengths { get; set; }
        [JsonIgnore]
        public bool GroupReusesKeyForTestCases { get; set; }
        [JsonIgnore]
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
