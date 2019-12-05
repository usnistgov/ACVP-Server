using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;
using System.Collections.Generic;

namespace NIST.CVP.Generation.SHA2.v1_0
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

        [JsonIgnore]
        public MathDomain MessageLength { get; set; }

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
