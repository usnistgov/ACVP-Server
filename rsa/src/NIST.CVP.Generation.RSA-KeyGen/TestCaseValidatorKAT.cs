using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseValidatorKAT : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorKAT(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        // KATs here give the client some data and expect them to determine if the pair is
        // a valid key pair. The expected response is just 'passed' or 'failed', so we check
        // if the client recognizes the test as a failed test.
        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.FailureTest != suppliedResult.FailureTest)
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
