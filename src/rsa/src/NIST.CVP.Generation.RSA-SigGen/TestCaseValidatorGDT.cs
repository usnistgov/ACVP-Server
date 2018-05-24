using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestGroup _serverGroup;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult> _deferredTestCaseResolver;
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorGDT(TestCase expectedResult, TestGroup serverGroup, IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult> resolver)
        {
            _serverGroup = serverGroup;
            _deferredTestCaseResolver = resolver;
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            Dictionary<string, string> expected = null;
            Dictionary<string, string> provided = null;

            if(_expectedResult.Message == null || suppliedResult.Signature == null)
            {
                errors.Add($"Could not find message or signature");
            }
            else
            {
                var result = _deferredTestCaseResolver.CompleteDeferredCrypto(_serverGroup, _expectedResult, suppliedResult);
                if (!result.Success)
                {
                    errors.Add($"Could not verify signature: {result.ErrorMessage}");
                    expected = new Dictionary<string, string>();
                    provided = new Dictionary<string, string>();
                }
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
    }
}
