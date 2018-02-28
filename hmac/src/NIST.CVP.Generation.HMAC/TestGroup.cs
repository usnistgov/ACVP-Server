using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.HMAC
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; } = "AFT";

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }

        [JsonProperty(PropertyName = "msgLen")]
        public int MessageLength { get; set; }

        [JsonProperty(PropertyName = "macLen")]
        public int MacLength { get; set; }

        public List<ITestCase> Tests { get; set; }

        [JsonIgnore]
        public ModeValues ShaMode { get; set; }

        [JsonIgnore]
        public DigestSizes ShaDigestSize { get; set; }

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
            KeyLength = expandoSource.GetTypeFromProperty<int>("keyLen");
            MessageLength = expandoSource.GetTypeFromProperty<int>("msgLen");
            MacLength = expandoSource.GetTypeFromProperty<int>("macLen");

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

            if (!int.TryParse(value, out var intVal))
            {
                return false;
            }

            // CAVS files in bytes, groups in bits
            switch (name.ToLower())
            {
                case "keylen":
                case "klen":
                    KeyLength = intVal * 8;
                    return true;
                case "msglen":
                case "mlen":
                    MessageLength = intVal * 8;
                    return true;
                case "maclen":
                case "tlen":
                    MacLength = intVal * 8;
                    return true;
            }
            return false;
        }
    }
}
