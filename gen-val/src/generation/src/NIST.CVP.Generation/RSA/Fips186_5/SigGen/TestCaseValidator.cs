using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Generation.RSA.Fips186_5.SigGen
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestGroup _serverGroup;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult> _deferredTestCaseResolver;
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(
            TestCase expectedResult,
            TestGroup serverGroup,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, VerifyResult> resolver)
        {
            _serverGroup = serverGroup;
            _deferredTestCaseResolver = resolver;
            _expectedResult = expectedResult;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            ValidateKey(suppliedResult, errors);
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
                    Reason = string.Join(";", errors),
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateKey(TestCase suppliedResult, List<string> errors)
        {
            var key = suppliedResult.ParentGroup.Key;

            if (key == null)
            {
                errors.Add("Could not find public key");
                return;
            }
            
            if (key.PubKey.N.ExactBitLength() != _serverGroup.Modulo)
            {
                errors.Add("N provided was not the correct size");
            }

            PrimeGeneratorGuard.AgainstInvalidPublicExponent(key.PubKey.E, errors);
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Signature == null)
            {
                errors.Add("Could not find signature");
            }

            if (_serverGroup.IsMessageRandomized && suppliedResult.RandomValue == null)
            {
                errors.Add($"{nameof(suppliedResult.RandomValue)} was not supplied");
            }

            if (_serverGroup.IsMessageRandomized && suppliedResult.RandomValueLen == 0)
            {
                errors.Add($"{nameof(suppliedResult.RandomValueLen)} was not supplied");
            }
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            var verifyResult = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(_serverGroup, _expectedResult, suppliedResult);
            if (!verifyResult.Success)
            {
                errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
            }
        }
    }
}