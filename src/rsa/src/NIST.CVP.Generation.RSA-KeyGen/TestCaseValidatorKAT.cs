using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorKat : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorKat(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        // KATs here give the client some data and expect them to determine if the pair is
        // a valid key pair. The expected response is just 'passed' or 'failed', so we check
        // if the client recognizes the test as a failed test.
        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.TestPassed != suppliedResult.TestPassed)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = "Incorrect response"
                };
            }

            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed};
        }
    }
}
