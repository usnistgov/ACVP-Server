using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CTR;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorCounterEncrypt : ITestCaseValidator<TestCase>
    {
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, CounterEncryptionResult> _deferredTestCaseResolver;
        private readonly TestCase _serverTestCase;
        private readonly TestGroup _group;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidatorCounterEncrypt(TestGroup group, TestCase testCase, IDeferredTestCaseResolver<TestGroup, TestCase, CounterEncryptionResult> resolver)
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
            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
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

            if (!serverResult.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Text does not match");
            }
        }
    }
}
