using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.PBKDF
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString DerivedKey { get; set; }

        [JsonProperty(PropertyName = "keyLen")]
        public int KeyLength { get; set; }
        public BitString Salt { get; set; }
        public string Password { get; set; }
        public int IterationCount { get; set; }
    }
}
