using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorNull : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly string _errorMessage;

        public int TestCaseId { get; }

        public TestCaseValidatorNull(string errorMessage, int testCaseId)
        {
            _errorMessage = errorMessage;
            TestCaseId = testCaseId;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            return new TestCaseValidation { Reason = _errorMessage, Result = Core.Enums.Disposition.Failed, TestCaseId = TestCaseId };
        }
    }
}
