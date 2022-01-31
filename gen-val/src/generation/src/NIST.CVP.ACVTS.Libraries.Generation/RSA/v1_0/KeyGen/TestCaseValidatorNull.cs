using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen
{
    public class TestCaseValidatorNull : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly string _errorMessage;

        public int TestCaseId { get; }

        public TestCaseValidatorNull(string errorMessage, int testCaseId)
        {
            _errorMessage = errorMessage;
            TestCaseId = testCaseId;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            return Task.FromResult(new TestCaseValidation
            {
                Reason = _errorMessage,
                Result = Core.Enums.Disposition.Failed,
                TestCaseId = TestCaseId
            });
        }
    }
}
