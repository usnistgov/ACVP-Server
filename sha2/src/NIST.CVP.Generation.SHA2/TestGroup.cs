using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestGroup : ITestGroup
    {
        public TestGroup()
        {
            Tests = new List<ITestCase>();
        }

        public TestGroup(JObject source) : this(source.ToObject<ExpandoObject>()) { }

        public TestGroup(dynamic source)
        {
            TestGroupId = (int) source.tgId;
            TestType = source.testType;
            Function = SHAEnumHelpers.StringToMode(source.function);
            DigestSize = SHAEnumHelpers.StringToDigest(source.digestSize);
            BitOriented = SetBoolValue(source, "inBit");
            IncludeNull = SetBoolValue(source, "inEmpty");

            Tests = new List<ITestCase>();
            foreach(var test in source.tests)
            {
                Tests.Add(new TestCase(test));
            }
        }

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

        private bool SetBoolValue(IDictionary<string, object> source, string label)
        {
            if (source.ContainsKey(label))
            {
                return (bool)source[label];
            }

            return default(bool);
        }
    }
}
