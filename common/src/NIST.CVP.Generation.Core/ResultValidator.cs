using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.Core
{
    public class ResultValidator<TTestCase> : IResultValidator<TTestCase>
        where TTestCase : ITestCase
    {
        
        public TestVectorValidation ValidateResults(List<ITestCaseValidator<TTestCase>> testCaseValidators, List<TTestCase> testResults)
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
