using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen
{
    public class TestCaseValidatorKat : ITestCaseValidatorAsync<TestGroup, TestCase>
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
        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            if (_expectedResult.TestPassed != suppliedResult.TestPassed)
            {
                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Disposition.Failed,
                    Reason = "Incorrect response"
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Disposition.Passed
            });
        }
    }
}