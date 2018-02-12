using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (!suppliedResult.FailureTest)
            {
                if(suppliedResult.Signature == null)
                {
                    errors.Add("Could not find signature");
                }
                else
                {
                    if (!_expectedResult.Signature.Equals(suppliedResult.Signature))
                    {
                        errors.Add("Signature does not match expected value");
                    }
                }
            }
            else
            {
                if (_expectedResult.FailureTest != suppliedResult.FailureTest)
                {
                    if (_expectedResult.FailureTest)
                    {
                        errors.Add("Test was expected to fail");
                    }
                    else
                    {
                        errors.Add("Test was not expected to fail");
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}
