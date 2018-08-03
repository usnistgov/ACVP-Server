using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.IKEv1
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

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.SKeyId.Equals(suppliedResult.SKeyId))
            {
                errors.Add($"{nameof(suppliedResult.SKeyId)} does not match");
                expected.Add(nameof(_serverTestCase.SKeyId), _serverTestCase.SKeyId.ToHex());
                provided.Add(nameof(suppliedResult.SKeyId), suppliedResult.SKeyId.ToHex());
            }

            if (!_serverTestCase.SKeyIdD.Equals(suppliedResult.SKeyIdD))
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdD)} does not match");
                expected.Add(nameof(_serverTestCase.SKeyIdD), _serverTestCase.SKeyIdD.ToHex());
                provided.Add(nameof(suppliedResult.SKeyIdD), suppliedResult.SKeyIdD.ToHex());
            }

            if (!_serverTestCase.SKeyIdA.Equals(suppliedResult.SKeyIdA))
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdA)} does not match");
                expected.Add(nameof(_serverTestCase.SKeyIdA), _serverTestCase.SKeyIdA.ToHex());
                provided.Add(nameof(suppliedResult.SKeyIdA), suppliedResult.SKeyIdA.ToHex());
            }

            if (!_serverTestCase.SKeyIdE.Equals(suppliedResult.SKeyIdE))
            {
                errors.Add($"{nameof(suppliedResult.SKeyIdE)} does not match");
                expected.Add(nameof(_serverTestCase.SKeyIdE), _serverTestCase.SKeyIdE.ToHex());
                provided.Add(nameof(suppliedResult.SKeyIdE), suppliedResult.SKeyIdE.ToHex());
            }
        }
    }
}
