using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestGroup _group;
        private readonly SignerBase _signer;
        public int TestCaseId { get; }

        public TestCaseValidatorGDT(TestCase expectedResult, TestGroup group, SignerBase signer)
        {
            _group = group;
            _signer = signer;
            TestCaseId = expectedResult.TestCaseId;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if(_group.Mode == SigGenModes.PSS)
            {
                _signer.AddEntropy(suppliedResult.Salt);
            }

            var result = _signer.Verify(_group.Modulo, suppliedResult.Signature, _group.Key, suppliedResult.Message);
            if (!result.Success)
            {
                errors.Add($"Could not verify signature for {TestCaseId}");
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }
    }
}
