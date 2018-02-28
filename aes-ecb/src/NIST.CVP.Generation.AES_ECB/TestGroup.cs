using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "MMT";
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "keylen")]
        public int KeyLength { get; set; }
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
            }
            return false;
        }
    }
}
