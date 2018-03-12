using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorNull<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        public int NumberOfTestCasesToGenerate => 0;

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, bool isSample)
        {
            return Generate(group, null);
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse<TTestGroup, TTestCase>("This is the null generator -- nothing is generated");
        }
    }
}
