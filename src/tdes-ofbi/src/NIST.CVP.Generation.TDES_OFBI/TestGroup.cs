using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>())
        {

        }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;
            if (expandoSource.ContainsProperty("keyingOption"))
            {
                KeyingOption = (int)source.keyingOption;
            }

            TestGroupId = (int) source.tgId;
            TestType = source.testType;
            Function = source.direction;
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }

        }
        
        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";
        [JsonProperty(PropertyName = "keyingOption")]
        public int KeyingOption { get; set; }
        public List<ITestCase> Tests { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "testtype":
                    TestType = value;
                    return true;
            }

            int intVal = 0;
            if (!int.TryParse(value, out intVal))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keyingoption":
                    KeyingOption = intVal;
                    return true;
            }
            return false;
        }
    }
}
