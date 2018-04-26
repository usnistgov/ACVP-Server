using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC
{
    public abstract class TestCaseBase<TTestGroup, TTestCase> : ITestCase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        public int TestCaseId { get; set; }
        public bool? TestPassed { get; set; }
        public bool Deferred => false;
        public TTestGroup ParentGroup { get; set; }
        public BitString Key { get; set; }
        public BitString Message { get; set; }
        public BitString Mac { get; set; }
        public abstract bool SetString(string name, string value);
    }
}