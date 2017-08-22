using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorGDT(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.Result == suppliedResult.Result)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
            }
            else
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = RSAEnumHelpers.FailureReasonToString(_expectedResult.Reason) };
            }
        }
    }
}
