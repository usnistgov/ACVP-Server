using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        // Properties for specific groups
        [JsonIgnore]
        public MathDomain DataLength { get; set; }

        // This is a vectorset / IUT property but it needs to be defined somewhere other than Parameter.cs
        [JsonProperty(PropertyName = "incremental")]
        public bool IncrementalCounter { get; set; }
        [JsonProperty(PropertyName = "overflow")]
        public bool OverflowCounter { get; set; }
        
        public bool SetString(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            switch (name.ToLower())
            {
                case "keylen":
                    KeyLength = Int32.Parse(value);
                    return true;

                case "direction":
                    Direction = value;
                    return true;
            }
            return false;
        }
    }
}
