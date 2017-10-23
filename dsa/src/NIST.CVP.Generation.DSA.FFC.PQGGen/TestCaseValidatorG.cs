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

            if (suppliedResult.G == 0)
            {
                errors.Add("Could not find g");
            }
            else
            {
                var validateResult = _gGen.Validate(_expectedResult.P, _expectedResult.Q, suppliedResult.G, _expectedResult.Seed, _expectedResult.Index);
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
