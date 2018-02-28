using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorNull<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase
        where TTestCase : TestCaseBase, new()
    {
        public int NumberOfTestCasesToGenerate => 1;
        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            return new TestCaseGenerateResponse("Null generator");
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse("Null generator");
        }
    }
}