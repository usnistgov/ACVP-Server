namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestCaseGenerator<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>, new()
    {
        public int NumberOfTestCasesToGenerate => 1;
        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, bool isSample)
        {
            TTestCase testCase = new TTestCase();

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse<TTestGroup, TTestCase> Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse<TTestGroup, TTestCase>(testCase);
        }
    }
}
