using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv2
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
            if (suppliedResult.SKeySeed == null)
            {
                errors.Add($"{nameof(suppliedResult.SKeySeed)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.DerivedKeyingMaterial == null)
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterial)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.DerivedKeyingMaterialChild == null)
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterialChild)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.DerivedKeyingMaterialDh == null)
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterialDh)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.SKeySeedReKey == null)
            {
                errors.Add($"{nameof(suppliedResult.SKeySeedReKey)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_serverTestCase.SKeySeed.Equals(suppliedResult.SKeySeed))
            {
                errors.Add($"{nameof(suppliedResult.SKeySeed)} does not match");
                expected.Add(nameof(_serverTestCase.SKeySeed), _serverTestCase.SKeySeed.ToHex());
                provided.Add(nameof(suppliedResult.SKeySeed), suppliedResult.SKeySeed.ToHex());
            }

            if (!_serverTestCase.DerivedKeyingMaterial.Equals(suppliedResult.DerivedKeyingMaterial))
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterial)} does not match");
                expected.Add(nameof(_serverTestCase.DerivedKeyingMaterial), _serverTestCase.DerivedKeyingMaterial.ToHex());
                provided.Add(nameof(suppliedResult.DerivedKeyingMaterial), suppliedResult.DerivedKeyingMaterial.ToHex());
            }

            if (!_serverTestCase.DerivedKeyingMaterialChild.Equals(suppliedResult.DerivedKeyingMaterialChild))
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterialChild)} does not match");
                expected.Add(nameof(_serverTestCase.DerivedKeyingMaterialChild), _serverTestCase.DerivedKeyingMaterialChild.ToHex());
                provided.Add(nameof(suppliedResult.DerivedKeyingMaterialChild), suppliedResult.DerivedKeyingMaterialChild.ToHex());
            }

            if (!_serverTestCase.DerivedKeyingMaterialDh.Equals(suppliedResult.DerivedKeyingMaterialDh))
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterialDh)} does not match");
                expected.Add(nameof(_serverTestCase.DerivedKeyingMaterialDh), _serverTestCase.DerivedKeyingMaterialDh.ToHex());
                provided.Add(nameof(suppliedResult.DerivedKeyingMaterialDh), suppliedResult.DerivedKeyingMaterialDh.ToHex());
            }

            if (!_serverTestCase.SKeySeedReKey.Equals(suppliedResult.SKeySeedReKey))
            {
                errors.Add($"{nameof(suppliedResult.SKeySeedReKey)} does not match");
                expected.Add(nameof(_serverTestCase.SKeySeedReKey), _serverTestCase.SKeySeedReKey.ToHex());
                provided.Add(nameof(suppliedResult.SKeySeedReKey), suppliedResult.SKeySeedReKey.ToHex());
            }
        }
    }
}
