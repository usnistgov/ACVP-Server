using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Crypto.RSA2.Signatures;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult> _deferredTestCaseResolver;
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorGDT(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult> resolver)
        {
            _group = group;
            _deferredTestCaseResolver = resolver;
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            
            if(_expectedResult.Message == null || suppliedResult.Signature == null)
            {
                errors.Add($"Could not find message or signature");
            }
            else
            {
                var result = _deferredTestCaseResolver.CompleteDeferredCrypto(_group, _expectedResult, suppliedResult);
                if (!result.Success)
                {
                    errors.Add($"Could not verify signature: {result.ErrorMessage}");
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}
