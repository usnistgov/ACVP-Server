using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }
        private readonly TestCase _expectedResult;

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.Result != suppliedResult.Result)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = _expectedResult.Reason.GetName() };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }
    }
}
