using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDeferredTestCaseResolver<TestGroup, TestCase, EccVerificationResult> _deferredResolver;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult, TestGroup group, IDeferredTestCaseResolver<TestGroup, TestCase, EccVerificationResult> deferredResolver)
        {
            _expectedResult = expectedResult;
            _group = group;
            _deferredResolver = deferredResolver;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();

            if (suppliedResult.Signature == null)
            {
                errors.Add("Could not find r or s");
            }
            else
            {
                var verifyResult = _deferredResolver.CompleteDeferredCrypto(_group, _expectedResult, suppliedResult);
                if (!verifyResult.Success)
                {
                    errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Disposition.Failed, 
                    Reason = string.Join(";", errors),
                    Expected = new Dictionary<string, string>(),
                    Provided = new Dictionary<string, string>()
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }
    }
}
