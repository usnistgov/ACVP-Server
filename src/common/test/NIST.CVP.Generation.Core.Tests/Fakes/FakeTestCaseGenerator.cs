namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestCaseGenerator<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase, new()
    {
        public int NumberOfTestCasesToGenerate => 1;
        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            TTestCase testCase = new TTestCase();

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse(testCase);
        }
    }
}
