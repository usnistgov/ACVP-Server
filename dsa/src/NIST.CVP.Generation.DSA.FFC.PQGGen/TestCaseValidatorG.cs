using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseValidatorG : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly IGGeneratorValidator _gGen;

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidatorG(TestCase expectedResult, IGGeneratorValidator gGen)
        {
            _expectedResult = expectedResult;
            _gGen = gGen;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            if (suppliedResult.G == null)
            {
                errors.Add("Could not find g");
            }
            else
            {
                var validateResult = _gGen.Validate(suppliedResult.P, suppliedResult.Q, suppliedResult.G, suppliedResult.Seed, suppliedResult.Index);
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
