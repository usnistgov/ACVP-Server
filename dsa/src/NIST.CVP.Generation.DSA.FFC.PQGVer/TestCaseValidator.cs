using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private TestCase _expectedResult;
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.FailureTest != suppliedResult.Result)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = _expectedResult.Reason };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }
    }
}
