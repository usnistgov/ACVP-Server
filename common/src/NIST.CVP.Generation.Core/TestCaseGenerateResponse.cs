namespace NIST.CVP.Generation.Core
{
    public class TestCaseGenerateResponse<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public TTestCase TestCase { get; }
        public string ErrorMessage { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public TestCaseGenerateResponse(TTestCase testCase)
        {
            TestCase = testCase;
        }

        public TestCaseGenerateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
