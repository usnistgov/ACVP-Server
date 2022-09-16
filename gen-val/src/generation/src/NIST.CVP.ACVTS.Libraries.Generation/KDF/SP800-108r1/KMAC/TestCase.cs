using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF.SP800_108r1.KMAC
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; }
        public bool Deferred { get; }
        
        public BitString KeyDerivationKey { get; set; }
        public BitString Context { get; set; }
        public BitString Label { get; set; }
        public int DerivedKeyLength { get; set; }
        public BitString DerivedKey { get; set; }
    }
}
