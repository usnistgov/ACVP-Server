using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly IDsaFfc _ffcDsa;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidator(TestCase expectedResult, IDsaFfc dsaFfc)
        {
            _expectedResult = expectedResult;
            _ffcDsa = dsaFfc;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (suppliedResult.Key.PrivateKeyX == 0 || suppliedResult.Key.PublicKeyY == 0)
            {
                errors.Add("Could not find x or y");
            }
            else if (suppliedResult.DomainParams.P == 0 || suppliedResult.DomainParams.Q == 0 || suppliedResult.DomainParams.G == 0)
            {
                errors.Add("Could not find p, q, or g");
            }
            else
            {
                var validateResult = _ffcDsa.ValidateKeyPair(suppliedResult.DomainParams, suppliedResult.Key);
                if (!validateResult.Success)
                {
                    errors.Add($"Validation failed: {validateResult.ErrorMessage}");
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
