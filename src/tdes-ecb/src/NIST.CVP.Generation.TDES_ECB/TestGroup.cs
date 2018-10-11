using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestGroup: ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        
        [JsonProperty(PropertyName = "direction")]
        public string Function { get; set; }
        
        [JsonProperty(PropertyName = "testType")]
        public string TestType{ get; set; }

        public string InternalTestType { get; set; } = "";

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
