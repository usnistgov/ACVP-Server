using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorNull<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase
        where TTestCase : TestCaseBase, new()
    {
        public int NumberOfTestCasesToGenerate { get { return 0; } }

        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            return Generate(group, null);
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse("This is the null generator -- nothing is generated");
        }
    }
}
