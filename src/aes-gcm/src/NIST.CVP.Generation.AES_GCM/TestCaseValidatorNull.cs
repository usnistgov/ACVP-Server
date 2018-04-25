using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseValidatorNull : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly string _errorMessage;

        public TestCaseValidatorNull(string errorMessage, int testCaseId)
        {
            _errorMessage = errorMessage;
            TestCaseId = testCaseId;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            return new TestCaseValidation() {Reason = _errorMessage, Result = Core.Enums.Disposition.Failed, TestCaseId = TestCaseId};
        }

        public int TestCaseId { get; }
    }
}
