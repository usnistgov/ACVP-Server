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
    public class TestGroup : ITestGroup<TestGroup, TestCase>
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

        public List<TestCase> Tests { get; set; } = new List<TestCase>();

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
