using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
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

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            if (!_serverTestCase.SKeySeed.Equals(suppliedResult.SKeySeed))
            {
                errors.Add($"{nameof(suppliedResult.SKeySeed)} does not match");
            }

            if (!_serverTestCase.DerivedKeyingMaterial.Equals(suppliedResult.DerivedKeyingMaterial))
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterial)} does not match");
            }

            if (!_serverTestCase.DerivedKeyingMaterialChild.Equals(suppliedResult.DerivedKeyingMaterialChild))
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterialChild)} does not match");
            }

            if (!_serverTestCase.DerivedKeyingMaterialDh.Equals(suppliedResult.DerivedKeyingMaterialDh))
            {
                errors.Add($"{nameof(suppliedResult.DerivedKeyingMaterialDh)} does not match");
            }

            if (!_serverTestCase.SKeySeedReKey.Equals(suppliedResult.SKeySeedReKey))
            {
                errors.Add($"{nameof(suppliedResult.SKeySeedReKey)} does not match");
            }
        }
    }
}
