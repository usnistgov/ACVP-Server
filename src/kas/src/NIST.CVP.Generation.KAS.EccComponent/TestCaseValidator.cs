using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KES;

namespace NIST.CVP.Generation.KAS.EccComponent
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _workingTest;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse> _deferredTestCaseResolver;

        public TestCaseValidator(TestCase workingTest, TestGroup group, IDeferredTestCaseResolver<TestGroup, TestCase, SharedSecretResponse> deferredTestCaseResolver)
        {
            _workingTest = workingTest;
            _group = group;
            _deferredTestCaseResolver = deferredTestCaseResolver;
        }

        public int TestCaseId => _workingTest.TestCaseId;

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
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

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = _deferredTestCaseResolver.CompleteDeferredCrypto(
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