using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS.v1_0.ECC_Component
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse> _deferredTestCaseResolver;

        public TestCaseValidator(
            TestCase workingTest, 
            TestGroup group, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, SharedSecretResponse> deferredTestCaseResolver
        )
        {
            _workingTest = workingTest;
            _group = group;
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public int TestCaseId => _workingTest.TestCaseId;

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                await CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.PublicKeyIutX == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.PublicKeyIutX)} but was not supplied");
            }
            if (suppliedResult.PublicKeyIutY == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.PublicKeyIutY)} but was not supplied");
            }
            
            if (suppliedResult.Z == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.Z)} but was not supplied");
            }
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(
                _group, 
                _workingTest, 
                suppliedResult
            );

            if (!serverResult.SharedSecretZ.Equals(suppliedResult.Z))
            {
                errors.Add($"{nameof(suppliedResult.Z)} does not match");
                expected.Add(nameof(serverResult.SharedSecretZ), serverResult.SharedSecretZ.ToHex());
                provided.Add(nameof(suppliedResult.Z), suppliedResult.Z.ToHex());
            }
        }
    }
}