using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseValidatorInternalEncrypt : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestGroup _testGroup;
        private readonly TestCase _serverTestCase;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult> _testCaseResolver;

        public TestCaseValidatorInternalEncrypt(
            TestGroup testGroup, 
            TestCase serverTestCase, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, AeadResult> testCaseResolver
        )
        {
            _serverTestCase = serverTestCase;
            _testGroup = testGroup;
            _testCaseResolver = testCaseResolver;
        }

        public int TestCaseId => _serverTestCase.TestCaseId;

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
                    Result = Disposition.Failed,
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var serverResult = await _testCaseResolver.CompleteDeferredCryptoAsync(_testGroup, _serverTestCase, suppliedResult);

            if (!serverResult.CipherText.Equals(suppliedResult.CipherText))
            {
                errors.Add("Cipher Text does not match");
                expected.Add(nameof(serverResult.CipherText), serverResult.CipherText.ToHex());
                provided.Add(nameof(suppliedResult.CipherText), suppliedResult.CipherText.ToHex());
            }
            if (!serverResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add("Tag does not match");
                expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                provided.Add(nameof(suppliedResult.Tag), suppliedResult.Tag.ToHex());
            }
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_testGroup.IvGeneration.ToLower() == "internal")
            {
                if (suppliedResult.IV == null)
                {
                    errors.Add($"{nameof(suppliedResult.IV)} was not present in the {nameof(TestCase)}");
                }
            }

            // When internal, validate the Salt is present in suppliedResults, otherwise use the expectedResults Salt
            if (_testGroup.SaltGen.ToLower() == "internal")
            {
                if (suppliedResult.Salt == null)
                {
                    errors.Add($"{nameof(suppliedResult.Salt)} was not present in the {nameof(TestCase)}");
                }
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
    }
}