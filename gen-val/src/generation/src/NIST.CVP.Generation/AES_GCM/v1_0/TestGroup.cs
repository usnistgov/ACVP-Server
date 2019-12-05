using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_GCM.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; } = "AFT";
        public string InternalTestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        [JsonProperty(PropertyName = "ivLen")]
        public int IvLength { get; set; }
        [JsonProperty(PropertyName = "ivGen")]
        public string IvGeneration { get; set; }
        [JsonProperty(PropertyName = "ivGenMode")]
        public string IvGenerationMode { get; set; }
        [JsonProperty(PropertyName = "payloadLen")]
        public int PayloadLength { get; set; }
        [JsonProperty(PropertyName = "aadLen")]
        public int AadLength { get; set; }
        [JsonProperty(PropertyName = "tagLen")]
        public int TagLength { get; set; }

        public AlgoMode AlgoMode { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = intVal;
                    return true;
                case "aadlen":
                    AadLength = intVal;
                    return true;
                case "taglen":
                    TagLength = intVal;
                    return true;
                case "ivlen":
                    IvLength = intVal;
                    return true;
                case "ptlen":
                    PayloadLength = intVal;
                    return true;
            }
            return false;
        }
    }
}
