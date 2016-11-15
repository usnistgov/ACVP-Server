using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class ResultValidator : IResultValidator
    {
        
        public TestVectorValidation ValidateResults(List<ITestCaseValidator> testCaseValidators, List<TestCase> testResults)
        {
            var validations = new List<TestCaseValidation>();
            foreach (var caseValidator in testCaseValidators)
            {
                var suppliedResult = testResults.FirstOrDefault(r => r.TestCaseId == caseValidator.TestCaseId);
                if (suppliedResult == null)
                {
                    validations.Add(new TestCaseValidation { TestCaseId = caseValidator.TestCaseId, Result = "missing" });
                    continue;
                }


                var validation = caseValidator.Validate(suppliedResult);
                validations.Add(validation);
            }

            return new TestVectorValidation { Validations = validations };
        }
    }
}
