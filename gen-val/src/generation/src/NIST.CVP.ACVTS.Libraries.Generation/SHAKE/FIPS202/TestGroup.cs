using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        [JsonProperty(PropertyName = "testType")]
        public string TestType { get; set; }

        [JsonIgnore]
        public MathDomain MessageLengths { get; set; }
        
        [JsonIgnore]
        public MathDomain OutputLength { get; set; }
        
        [JsonIgnore]
        public HashFunction HashFunction { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
