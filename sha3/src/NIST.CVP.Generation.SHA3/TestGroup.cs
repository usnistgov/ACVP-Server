using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.SHA3
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        public List<ITestCase> Tests { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "function")]
        public string Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public int DigestSize { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOrientedInput { get; set; } = false;

        [JsonProperty(PropertyName = "outBit")]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; } = false;

        [JsonProperty(PropertyName = "outputLength")]
        public MathDomain OutputLength { get; set; }

        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            var expandoSource = (ExpandoObject) source;

            TestGroupId = expandoSource.GetTypeFromProperty<int>("tgId");
            TestType = expandoSource.GetTypeFromProperty<string>("testType");
            Function = expandoSource.GetTypeFromProperty<string>("function");
            DigestSize = expandoSource.GetTypeFromProperty<int>("digestSize");
            BitOrientedInput = expandoSource.GetTypeFromProperty<bool>("inBit");
            BitOrientedOutput = expandoSource.GetTypeFromProperty<bool>("outBit");
            IncludeNull = expandoSource.GetTypeFromProperty<bool>("inEmpty");
            
            Tests = new List<ITestCase>();
            foreach (var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            name = name.ToLower();

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
                case "function":
                    Function = value;
                    return true;
            }

            return false;
        }
    }
}
