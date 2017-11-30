using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorCounterDecrypt : ITestCaseValidator<TestCase>
    {
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult> _deferredTestCaseResolver;
        private readonly TestCase _serverTestCase;
        private readonly TestGroup _group;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidatorCounterDecrypt(TestGroup group, TestCase testCase, IDeferredTestCaseResolver<TestGroup, TestCase, CounterDecryptionResult> resolver)
        {
            _serverTestCase = testCase;
            _deferredTestCaseResolver = resolver;
            _group = group;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.PlainText == null)
            {
                errors.Add($"{nameof(suppliedResult.PlainText)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.IVs == null)
            {
                errors.Add($"{nameof(suppliedResult.IVs)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.IVs.Count != _serverTestCase.Length / 128)
            {
                errors.Add($"{nameof(suppliedResult.IVs)} does not have the correct number of values");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            var serverResult = _deferredTestCaseResolver.CompleteDeferredCrypto(_group, _serverTestCase, suppliedResult);

            if (!serverResult.Success)
            {
                errors.Add($"Server unable to complete test case with error: {serverResult.ErrorMessage}");
                return;
            }

            if (!serverResult.PlainText.Equals(suppliedResult.PlainText))
            {
                errors.Add("Plain Text does not match");
            }
        }
    }
}
