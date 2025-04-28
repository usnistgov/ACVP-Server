using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.XECDH.RFC7748.KeyGen
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, XecdhKeyPairGenerateResult> _deferredResolver;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(
            TestCase expectedResult,
            TestGroup group,
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, XecdhKeyPairGenerateResult> deferredResolver
        )
        {
            _expectedResult = expectedResult;
            _group = group;
            _deferredResolver = deferredResolver;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (suppliedResult.KeyPair.PrivateKey.ToPositiveBigInteger() == 0 || suppliedResult.KeyPair.PublicKey.ToPositiveBigInteger() == 0)
            {
                errors.Add("Could not find value in key pair");
            }
            else
            {
                var deferredResult = await _deferredResolver.CompleteDeferredCryptoAsync(_group, _expectedResult, suppliedResult);
                if (!deferredResult.Success)
                {
                    errors.Add("Unable to generate public key from private key");
                }
                else
                {
                    if (!deferredResult.KeyPair.PublicKey.Equals(suppliedResult.KeyPair.PublicKey))
                    {
                        errors.Add("Incorrect public key generated from private key");
                        expected.Add(nameof(deferredResult.KeyPair.PublicKey), deferredResult.KeyPair.PublicKey.ToHex());
                        provided.Add(nameof(suppliedResult.KeyPair.PublicKey), suppliedResult.KeyPair.PublicKey.ToHex());
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Disposition.Failed,
                    Reason = string.Join(";", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }
    }
}
