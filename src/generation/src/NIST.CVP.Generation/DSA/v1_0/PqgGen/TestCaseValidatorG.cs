using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.v1_0.PqgGen
{
    public class TestCaseValidatorG : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolverAsync<TestGroup, TestCase, GValidateResult> _deferredResolver;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorG(
            TestCase expectedResult, 
            TestGroup group, 
            IDeferredTestCaseResolverAsync<TestGroup, TestCase, GValidateResult> deferredResolver
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

            if (suppliedResult.G == 0)
            {
                errors.Add("Could not find g");
            }
            else
            {
                var validateResult = await _deferredResolver.CompleteDeferredCryptoAsync(_group, _expectedResult, suppliedResult);
                if (!validateResult.Success)
                {
                    errors.Add($"Validation failed: {validateResult.ErrorMessage}");
                    expected.Add(nameof(validateResult.Success), validateResult.Success.ToString());
                    provided.Add(nameof(validateResult.Success), (!validateResult.Success).ToString());
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
