using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_XPN
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
        public int IVLength => 96;
        [JsonProperty(PropertyName = "ivGen")]
        public string IVGeneration { get; set; }
        [JsonProperty(PropertyName = "ivGenMode")]
        public string IVGenerationMode { get; set; }
        [JsonProperty(PropertyName = "saltLen")]
        public int SaltLength => 96;
        [JsonProperty(PropertyName = "saltGen")]
        public string SaltGen { get; set; }
        [JsonProperty(PropertyName = "ptLen")]
        public int PTLength { get; set; }
        [JsonProperty(PropertyName = "aadLen")]
        public int AADLength { get; set; }
        [JsonProperty(PropertyName = "tagLen")]
        public int TagLength { get; set; }
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
                    AADLength = intVal;
                    return true;
                case "taglen":
                    TagLength = intVal;
                    return true;
                case "ptlen":
                    PTLength = intVal;
                    return true;
            }
            return false;
        }
    }
}
