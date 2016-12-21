using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.Tests.Fakes
{
    public class TestCaseValidatorFake : ITestCaseValidator<TestCase>
    {
        public int TestCaseId { get; set; }
        private readonly string _result;
        public TestCaseValidatorFake(string result)
        {
            _result = result;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            return new TestCaseValidation {TestCaseId = TestCaseId, Result = _result};
        }
    }
}
