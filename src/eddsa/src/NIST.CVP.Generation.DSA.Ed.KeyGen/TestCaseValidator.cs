using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdKeyPairGenerateResult> _deferredResolver;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(
            TestCase expectedResult, 
            TestGroup group, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, EdKeyPairGenerateResult> deferredResolver
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

            if (suppliedResult.KeyPair.PrivateD.ToPositiveBigInteger() == 0 || suppliedResult.KeyPair.PublicQ.ToPositiveBigInteger() == 0)
            {
                errors.Add("Could not find value in key pair");
            }
            else
            {
                var deferredResult = await _deferredResolver.CompleteDeferredCryptoAsync(_group, _expectedResult, suppliedResult);
                if (!deferredResult.Success)
                {
                    errors.Add("Unable to generate public key from private key d value");
                }
                else
                {
                    if (!deferredResult.KeyPair.PublicQ.Equals(suppliedResult.KeyPair.PublicQ))
                    {
                        errors.Add("Incorrect Q generated from private key");
                        expected.Add(nameof(deferredResult.KeyPair.PublicQ), deferredResult.KeyPair.PublicQ.ToHex());
                        provided.Add(nameof(suppliedResult.KeyPair.PublicQ), suppliedResult.KeyPair.PublicQ.ToHex());
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
