using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.HKDF.v1_0
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString InputKeyingMaterial { get; set; }
        public BitString Salt { get; set; }
        public BitString OtherInfo { get; set; }

        public int KeyLength { get; set; }
        public BitString DerivedKey { get; set; }
    }
}
