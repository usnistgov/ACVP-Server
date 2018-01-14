using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestGroup _group;
        private readonly SignerBase _signer;
        private readonly TestCase _expectedResult;
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorGDT(TestCase expectedResult, TestGroup group, SignerBase signer)
        {
            _group = group;
            _signer = signer;
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if(_group.Mode == SigGenModes.PSS)
            {
                _signer.AddEntropy(suppliedResult.Salt);
            }

            if(_expectedResult.Message == null || suppliedResult.Signature == null)
            {
                errors.Add($"Could not find message or signature");
            }
            else
            {
                var result = _signer.Verify(_group.Modulo, suppliedResult.Signature, suppliedResult.Key, _expectedResult.Message);
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
