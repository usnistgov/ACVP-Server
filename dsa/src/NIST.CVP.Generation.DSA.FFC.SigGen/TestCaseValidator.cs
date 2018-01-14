using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDsaFfc _ffcDsa;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidator(TestCase expectedResult, TestGroup group, IDsaFfc dsaFfc)
        {
            _expectedResult = expectedResult;
            _group = group;
            _ffcDsa = dsaFfc;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (suppliedResult.Signature == null)
            {
                errors.Add("Could not find r or s");
            }
            else if (suppliedResult.DomainParams == null)
            {
                errors.Add("Could not find p, q or g");
            }
            else
            {
                var verifyResult = _ffcDsa.Verify(suppliedResult.DomainParams, suppliedResult.Key, _expectedResult.Message, suppliedResult.Signature);
                if (!verifyResult.Success)
                {
                    errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
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
