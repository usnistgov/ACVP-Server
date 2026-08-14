using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        public int TestGroupId { get; set; }

        public string TestType { get; set; }

        [JsonIgnore]
        public MathDomain DigestLength { get; set; }

        [JsonIgnore]
        public MathDomain MessageLength { get; set; }

        [JsonIgnore]
        public MathDomain KeyLength { get; set; }

        public List<TestCase> Tests { get; set; } = new List<TestCase>();
    }
}
