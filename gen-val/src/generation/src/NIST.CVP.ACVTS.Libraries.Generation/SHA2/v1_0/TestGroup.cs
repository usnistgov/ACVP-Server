using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "function")]
        public ModeValues Function { get; set; }

        [JsonProperty(PropertyName = "digestSize")]
        public DigestSizes DigestSize { get; set; }
        
        [JsonProperty(PropertyName = "mctVersion")]
        public MctVersions MctVersion { get; set; }
        
        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public MathDomain MessageLength { get; set; }

        [JsonIgnore]
        public HashFunction CommonHashFunction => new HashFunction(Function, DigestSize);

        [JsonIgnore]
        public int[] LargeDataSizes { get; set; }

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
                    Function = ShaAttributes.StringToMode(value);
                    return true;
                }

                if (name == "digestsize")
                {
                    DigestSize = ShaAttributes.StringToDigest(value);
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
