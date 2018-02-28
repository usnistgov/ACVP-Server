using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keylen")]
        public int KeyLength { get; set; }

        [JsonProperty(PropertyName = "ivlen")]
        public int IVLength => 96;
        [JsonProperty(PropertyName = "ivgen")]
        public string IVGeneration { get; set; }
        [JsonProperty(PropertyName = "ivgenmode")]
        public string IVGenerationMode { get; set; }
        [JsonProperty(PropertyName = "saltlen")]
        public int SaltLength => 96;
        [JsonProperty(PropertyName = "saltgen")]
        public string SaltGen { get; set; }
        [JsonProperty(PropertyName = "ptlen")]
        public int PTLength { get; set; }
        [JsonProperty(PropertyName = "aadlen")]
        public int AADLength { get; set; }
        [JsonProperty(PropertyName = "taglen")]
        public int TagLength { get; set; }
        public List<ITestCase> Tests { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject)source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            IVGeneration = expandoSource.GetTypeFromProperty<string>("ivGen");
            IVGenerationMode = expandoSource.GetTypeFromProperty<string>("ivGenMode");
            SaltGen = expandoSource.GetTypeFromProperty<string>("saltGen");
            AADLength = expandoSource.GetTypeFromProperty<int>("aadLen");
            PTLength = expandoSource.GetTypeFromProperty<int>("ptLen");
            TagLength = expandoSource.GetTypeFromProperty<int>("tagLen");
            KeyLength = expandoSource.GetTypeFromProperty<int>("keyLen");
            Function = expandoSource.GetTypeFromProperty<string>("direction");
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }
        
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
