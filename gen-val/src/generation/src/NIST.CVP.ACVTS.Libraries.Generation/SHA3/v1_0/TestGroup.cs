using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public ModeValues Function { get; set; }

        [JsonIgnore]
        public DigestSizes DigestSize { get; set; }

        [JsonIgnore]
        public HashFunction CommonHashFunction => new HashFunction(Function, DigestSize);

        [JsonIgnore]
        public bool BitOrientedInput { get; set; } = false;

        [JsonIgnore]
        public bool BitOrientedOutput { get; set; } = false;

        [JsonIgnore]
        public bool IncludeNull { get; set; } = false;

        [JsonIgnore]
        public MathDomain OutputLength { get; set; }

        [JsonProperty(PropertyName = "maxOutLen")]
        public int MaxOutputLength => OutputLength?.GetDomainMinMax().Maximum ?? 0;

        [JsonProperty(PropertyName = "minOutLen")]
        public int MinOutputLength => OutputLength?.GetDomainMinMax().Minimum ?? 0;

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

            switch (name)
            {
                case "testtype":
                    TestType = value;
                    return true;
                case "function":
                    Function = ShaAttributes.StringToMode(value);
                    return true;
            }

            return false;
        }
    }
}
