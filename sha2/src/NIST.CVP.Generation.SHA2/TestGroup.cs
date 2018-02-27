using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.SHA2
{
    public class TestGroup : ITestGroup
    {
        public int TestGroupId { get; set; }
        [JsonProperty(PropertyName = "function")]
        public ModeValues Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public DigestSizes DigestSize { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonProperty(PropertyName = "inBit")]
        public bool BitOriented { get; set; }

        [JsonProperty(PropertyName = "inEmpty")]
        public bool IncludeNull { get; set; }

        public List<ITestCase> Tests { get; set; }

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

            var functionValue = expandoSource.GetTypeFromProperty<string>("function");
            if (functionValue != default(string))
            {
                Function = SHAEnumHelpers.StringToMode(functionValue);
            }

            var digestValue = expandoSource.GetTypeFromProperty<string>("digestSize");
            if (digestValue != default(string))
            {
                DigestSize = SHAEnumHelpers.StringToDigest(digestValue);
            }

            BitOriented = expandoSource.GetTypeFromProperty<bool>("inBit");
            IncludeNull = expandoSource.GetTypeFromProperty<bool>("inEmpty");
            
            Tests = new List<ITestCase>();
            foreach(var test in source.tests)
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

            try
            {
                if (name == "testtype")
                {
                    TestType = value;
                    return true;
                }

                if (name == "function")
                {
                    Function = SHAEnumHelpers.StringToMode(value);
                    return true;
                }

                if (name == "digestsize")
                {
                    DigestSize = SHAEnumHelpers.StringToDigest(value);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}
