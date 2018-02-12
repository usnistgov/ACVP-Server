using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, EncryptionResult> _deferredTestCaseResolver;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult, IDeferredTestCaseResolver<TestGroup, TestCase, EncryptionResult> resolver)
        {
            _expectedResult = expectedResult;
            _deferredTestCaseResolver = resolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            var computedResult = _deferredTestCaseResolver.CompleteDeferredCrypto(null, _expectedResult, suppliedResult);

            if (computedResult.Success)
            {
                if (suppliedResult.FailureTest)
                {
                    errors.Add("Test was expected to pass");
                }
                else
                {
                    if(suppliedResult.PlainText == null)
                    {
                        errors.Add("Could not find plainText");
                    }
                    else
                    {
                        if (!_expectedResult.PlainText.Equals(suppliedResult.PlainText))
                        {
                            errors.Add("PlainText does not match expected value");
                        }
                    }
                }
            }
            else
            {
                if (!suppliedResult.FailureTest)
                {
                    errors.Add("Test was expected to fail");
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
