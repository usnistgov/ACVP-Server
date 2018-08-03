using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA_SigGen
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

            if(_expectedResult.Message == null || suppliedResult.Signature == null)
            {
                errors.Add("Could not find message or signature");
            }
            else
            {
                var result = await _deferredTestCaseResolver.CompleteDeferredCryptoAsync(_serverGroup, _expectedResult, suppliedResult);
                if (!result.Success)
                {
                    errors.Add($"Could not verify signature: {result.ErrorMessage}");
                    expected.Add(nameof(result.Success), (!result.Success).ToString());
                    provided.Add(nameof(result.Success), result.Success.ToString());
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join(";", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}
