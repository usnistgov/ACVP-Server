using Newtonsoft.Json;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
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