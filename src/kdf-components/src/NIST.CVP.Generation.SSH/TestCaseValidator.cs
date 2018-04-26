using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SSH
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase)
        {
            _serverTestCase = serverTestCase;
        }

        public TestCaseValidation Validate(TestCase iutResult)
        {
            var errors = new List<string>();

            ValidateResultPresent(iutResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(iutResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
            }

            return new TestCaseValidation { TestCaseId = TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.InitialIvClient == null)
            {
                errors.Add($"{nameof(suppliedResult.InitialIvClient)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.EncryptionKeyClient == null)
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyClient)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.IntegrityKeyClient == null)
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyClient)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.InitialIvServer == null)
            {
                errors.Add($"{nameof(suppliedResult.InitialIvServer)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.EncryptionKeyServer == null)
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyServer)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.IntegrityKeyServer == null)
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyServer)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_serverTestCase.InitialIvClient.Equals(suppliedResult.InitialIvClient))
            {
                errors.Add($"{nameof(suppliedResult.InitialIvClient)} does not match");
            }

            if (!_serverTestCase.EncryptionKeyClient.Equals(suppliedResult.EncryptionKeyClient))
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyClient)} does not match");
            }

            if (!_serverTestCase.IntegrityKeyClient.Equals(suppliedResult.IntegrityKeyClient))
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyClient)} does not match");
            }

            if (!_serverTestCase.InitialIvServer.Equals(suppliedResult.InitialIvServer))
            {
                errors.Add($"{nameof(suppliedResult.InitialIvServer)} does not match");
            }

            if (!_serverTestCase.EncryptionKeyServer.Equals(suppliedResult.EncryptionKeyServer))
            {
                errors.Add($"{nameof(suppliedResult.EncryptionKeyServer)} does not match");
            }

            if (!_serverTestCase.IntegrityKeyServer.Equals(suppliedResult.IntegrityKeyServer))
            {
                errors.Add($"{nameof(suppliedResult.IntegrityKeyServer)} does not match");
            }
        }
    }
}
