using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string InternalTestType { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "KAT";

        [JsonProperty(PropertyName = "keyingOption")]
        public int KeyingOption { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

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

            if (!int.TryParse(value, out var intVal))
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
