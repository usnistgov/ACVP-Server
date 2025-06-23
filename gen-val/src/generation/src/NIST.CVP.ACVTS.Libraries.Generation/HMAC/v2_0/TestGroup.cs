using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.v2_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        public string TestType { get; set; }

        [JsonIgnore]
        public MathDomain KeyLen { get; set; }

        [JsonIgnore]
        public MathDomain MacLen { get; set; }
        
        [JsonIgnore]
        public MathDomain MessageLen { get; set; }
        
        public List<TestCase> Tests { get; set; } = [];

        [JsonIgnore]
        public ModeValues ShaMode { get; init; }

        [JsonIgnore]
        public DigestSizes ShaDigestSize { get; init; }
    }
}
