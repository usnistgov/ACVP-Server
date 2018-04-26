using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SRTP
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
            if (suppliedResult.SrtpKe == null)
            {
                errors.Add($"{nameof(suppliedResult.SrtpKe)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SrtpKa == null)
            {
                errors.Add($"{nameof(suppliedResult.SrtpKa)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SrtpKs == null)
            {
                errors.Add($"{nameof(suppliedResult.SrtpKs)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SrtcpKe == null)
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKe)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SrtcpKa == null)
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKa)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SrtcpKs == null)
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKs)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_serverTestCase.SrtpKe.Equals(suppliedResult.SrtpKe))
            {
                errors.Add($"{nameof(suppliedResult.SrtpKe)} does not match");
            }

            if (!_serverTestCase.SrtpKa.Equals(suppliedResult.SrtpKa))
            {
                errors.Add($"{nameof(suppliedResult.SrtpKa)} does not match");
            }

            if (!_serverTestCase.SrtpKs.Equals(suppliedResult.SrtpKs))
            {
                errors.Add($"{nameof(suppliedResult.SrtpKs)} does not match");
            }

            if (!_serverTestCase.SrtcpKe.Equals(suppliedResult.SrtcpKe))
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKe)} does not match");
            }

            if (!_serverTestCase.SrtcpKa.Equals(suppliedResult.SrtcpKa))
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKa)} does not match");
            }

            if (!_serverTestCase.SrtcpKs.Equals(suppliedResult.SrtcpKs))
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKs)} does not match");
            }
        }
    }
}
