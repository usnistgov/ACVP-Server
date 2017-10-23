using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseValidatorInternalEncrypt : ITestCaseValidator<TestCase>
    {
        private TestCase _expectedResult;
        private readonly TestGroup _testGroup;
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseValidatorInternalEncrypt(TestCase expectedResult, TestGroup testGroup, ITestCaseGeneratorFactory<TestGroup, TestCase> testCaseGeneratorFactory)
        {
            _expectedResult = expectedResult;
            _testGroup = testGroup;
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            List<string> errors = new List<string>();
            var generator = _testCaseGeneratorFactory.GetCaseGenerator(_testGroup);

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                var newResult = generator.Generate(_testGroup, _expectedResult);
                if (newResult.Success)
                {
                    var expectedResult = (TestCase)newResult.TestCase;

                    CheckResults(expectedResult, suppliedResult, errors);
                }
                else
                {
                    errors.Add("Failed generating TestCase on inputs");
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_testGroup.IVGeneration.ToLower() == "internal")
            {
                if (suppliedResult.IV == null)
                {
                    errors.Add($"{nameof(suppliedResult.IV)} was not present in the {nameof(TestCase)}");
                }

                _expectedResult.IV = suppliedResult.IV;
            }

            // When internal, validate the Salt is present in suppliedResults, otherwise use the expectedResults Salt
            if (_testGroup.SaltGen.ToLower() == "internal")
            {
                if (suppliedResult.Salt == null)
                {
                    errors.Add($"{nameof(suppliedResult.Salt)} was not present in the {nameof(TestCase)}");
                }

                _expectedResult.IV = suppliedResult.IV;
            }

            if (suppliedResult.CipherText == null)
            {
                errors.Add($"{nameof(suppliedResult.CipherText)} was not present in the {nameof(TestCase)}");
            }

            if (suppliedResult.Tag == null)
            {
                errors.Add($"{nameof(suppliedResult.Tag)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase expectedResults, TestCase suppliedResult, List<string> errors)
        {
            if (!expectedResults.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Text does not match");
            }
            if (!expectedResults.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add("Tag does not match");
            }
        }
    }
}