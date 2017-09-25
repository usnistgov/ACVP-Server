using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseValidatorPQ : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly IPQGeneratorValidator _pqGen;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorPQ(TestCase expectedResult, IPQGeneratorValidator pqGen)
        {
            _expectedResult = expectedResult;
            _pqGen = pqGen;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (suppliedResult.P == 0 || suppliedResult.Q == 0)
            {
                errors.Add("Could not find p or q");
            }
            else
            {
                var validateResult = _pqGen.Validate(suppliedResult.P, suppliedResult.Q, _expectedResult.Seed, suppliedResult.Counter);
                if (!validateResult.Success)
                {
                    errors.Add($"Validation failed: {validateResult.ErrorMessage}");
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join(";", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }
    }
}
