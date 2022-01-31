using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestCase : ITestCase<TestGroup, TestCase>
    {
        public int TestCaseId { get; set; }
        public TestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred { get; set; }

        public BitString Zz { get; set; }
        public BitString DerivedKey { get; set; }
        public int KeyLen => DerivedKey.BitLength;
        public BitString OtherInfo { get; set; }
        public BitString PartyUInfo { get; set; }
        public BitString PartyVInfo { get; set; }
        public BitString SuppPubInfo { get; set; }
        public BitString SuppPrivInfo { get; set; }
    }
}
