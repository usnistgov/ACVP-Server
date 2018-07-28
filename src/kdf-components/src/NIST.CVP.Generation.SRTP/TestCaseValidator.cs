using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.SRTP
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _serverTestCase;
        public int TestCaseId => _serverTestCase.TestCaseId;

        public TestCaseValidator(TestCase serverTestCase)
        {
            _serverTestCase = serverTestCase;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase iutResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(iutResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(iutResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return Task.FromResult(new TestCaseValidation 
                { 
                    TestCaseId = TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = TestCaseId,
                Result = Core.Enums.Disposition.Passed
            });
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

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.SrtpKe.Equals(suppliedResult.SrtpKe))
            {
                errors.Add($"{nameof(suppliedResult.SrtpKe)} does not match");
                expected.Add(nameof(_serverTestCase.SrtpKe), _serverTestCase.SrtpKe.ToHex());
                provided.Add(nameof(suppliedResult.SrtpKe), suppliedResult.SrtpKe.ToHex());
            }

            if (!_serverTestCase.SrtpKa.Equals(suppliedResult.SrtpKa))
            {
                errors.Add($"{nameof(suppliedResult.SrtpKa)} does not match");
                expected.Add(nameof(_serverTestCase.SrtpKa), _serverTestCase.SrtpKa.ToHex());
                provided.Add(nameof(suppliedResult.SrtpKa), suppliedResult.SrtpKa.ToHex());
            }

            if (!_serverTestCase.SrtpKs.Equals(suppliedResult.SrtpKs))
            {
                errors.Add($"{nameof(suppliedResult.SrtpKs)} does not match");
                expected.Add(nameof(_serverTestCase.SrtpKs), _serverTestCase.SrtpKs.ToHex());
                provided.Add(nameof(suppliedResult.SrtpKs), suppliedResult.SrtpKs.ToHex());
            }

            if (!_serverTestCase.SrtcpKe.Equals(suppliedResult.SrtcpKe))
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKe)} does not match");
                expected.Add(nameof(_serverTestCase.SrtcpKe), _serverTestCase.SrtcpKe.ToHex());
                provided.Add(nameof(suppliedResult.SrtcpKe), suppliedResult.SrtcpKe.ToHex());
            }

            if (!_serverTestCase.SrtcpKa.Equals(suppliedResult.SrtcpKa))
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKa)} does not match");
                expected.Add(nameof(_serverTestCase.SrtcpKa), _serverTestCase.SrtcpKa.ToHex());
                provided.Add(nameof(suppliedResult.SrtcpKa), suppliedResult.SrtcpKa.ToHex());
            }

            if (!_serverTestCase.SrtcpKs.Equals(suppliedResult.SrtcpKs))
            {
                errors.Add($"{nameof(suppliedResult.SrtcpKs)} does not match");
                expected.Add(nameof(_serverTestCase.SrtcpKs), _serverTestCase.SrtcpKs.ToHex());
                provided.Add(nameof(suppliedResult.SrtcpKs), suppliedResult.SrtcpKs.ToHex());
            }
        }
    }
}
