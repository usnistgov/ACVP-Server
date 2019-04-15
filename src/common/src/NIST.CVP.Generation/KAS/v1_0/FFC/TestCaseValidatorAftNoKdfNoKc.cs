using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KAS.v1_0.FFC
{
    public class TestCaseValidatorAftNoKdfNoKc : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _workingResult;
        private readonly TestGroup _testGroup;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult> _deferredResolver;

        public TestCaseValidatorAftNoKdfNoKc(
            TestCase workingResult, 
            TestGroup testGroup, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult> deferredResolver)
        {
            _workingResult = workingResult;
            _testGroup = testGroup;
            _deferredResolver = deferredResolver;
        }

        public int TestCaseId => _workingResult.TestCaseId;

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
            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesStaticKeyPair)
            {
                if (suppliedResult.StaticPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIut)} but was not supplied");
                }
            }

            if (_testGroup.KeyNonceGenRequirementsIut.GeneratesEphemeralKeyPair)
            {
                if (suppliedResult.EphemeralPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIut)} but was not supplied");
                }
            }

            if (suppliedResult.HashZ == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.HashZ)} but was not supplied");
            }
        }

        private async Task CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            KasResult serverResult = await _deferredResolver.CompleteDeferredCryptoAsync(_testGroup, _workingResult, suppliedResult);

            if (!serverResult.Tag.Equals(suppliedResult.HashZ))
            {
                errors.Add($"{nameof(suppliedResult.HashZ)} does not match");
                expected.Add(nameof(serverResult.Tag), serverResult.Tag.ToHex());
                provided.Add(nameof(suppliedResult.HashZ), suppliedResult.HashZ.ToHex());
            }
        }
    }
}