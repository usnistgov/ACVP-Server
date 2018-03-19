using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorNull<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        public int NumberOfTestCasesToGenerate => 1;

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup group, bool isSample)
        {
            return new TestCaseGenerateResponse<TTestGroup, TTestCase>("Null generator");
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse<TTestGroup, TTestCase>("Null generator");
        }
    }
}