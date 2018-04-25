using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap
{
    public abstract class TestCaseBase<TTestGroup, TTestCase> : ITestCase<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public int TestCaseId { get; set; }
        public TTestGroup ParentGroup { get; set; }
        public bool? TestPassed { get; set; } = true;
        public bool Deferred { get; set; }
        public BitString Key { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }

        public abstract bool SetString(string name, string value);
    }
}
