using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.IKEv1
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
            if (suppliedResult.SKeyId == null)
            {
                errors.Add($"{nameof(suppliedResult.SKeyId)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SKeyIdD == null)
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdD)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SKeyIdA == null)
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdA)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SKeyIdE == null)
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdE)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_serverTestCase.SKeyId.Equals(suppliedResult.SKeyId))
            {
                errors.Add($"{nameof(suppliedResult.SKeyId)} does not match");
            }

            if (!_serverTestCase.SKeyIdD.Equals(suppliedResult.SKeyIdD))
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdD)} does not match");
            }

            if (!_serverTestCase.SKeyIdA.Equals(suppliedResult.SKeyIdA))
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdA)} does not match");
            }

            if (!_serverTestCase.SKeyIdE.Equals(suppliedResult.SKeyIdE))
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdE)} does not match");
            }
        }
    }
}
