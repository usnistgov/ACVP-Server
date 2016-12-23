namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestCaseValidator<TTestCase> : ITestCaseValidator<TTestCase>
        where TTestCase : ITestCase
    {
        public int TestCaseId { get; set; }
        private readonly string _result;
        public FakeTestCaseValidator(string result)
        {
            _result = result;
        }

        public TestCaseValidation Validate(TTestCase suppliedResult)
        {
            return new TestCaseValidation {TestCaseId = TestCaseId, Result = _result};
        }
    }
}
