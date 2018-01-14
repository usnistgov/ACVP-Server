using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _group;
        private readonly IDsaEcc _eccDsa;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidator(TestCase expectedResult, TestGroup group, IDsaEcc eccDsa)
        {
            _expectedResult = expectedResult;
            _group = group;
            _eccDsa = eccDsa;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (suppliedResult.Signature == null)
            {
                errors.Add("Could not find r or s");
            }
            else if (suppliedResult.KeyPair == null)
            {
                errors.Add("Could not find Q");
            }
            else
            {
                var verifyResult = _eccDsa.Verify(_group.DomainParameters, suppliedResult.KeyPair, _expectedResult.Message, suppliedResult.Signature);
                if (!verifyResult.Success)
                {
                    errors.Add($"Validation failed: {verifyResult.ErrorMessage}");
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Failed, Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }
    }
}
