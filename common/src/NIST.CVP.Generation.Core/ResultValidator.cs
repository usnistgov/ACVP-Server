using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class ResultValidator<TTestCase> : IResultValidator<TTestCase>
        where TTestCase : ITestCase
    {
        
        public TestVectorValidation ValidateResults(IEnumerable<ITestCaseValidator<TTestCase>> testCaseValidators, IEnumerable<TTestCase> testResults)
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


                try
                {
                    var validation = caseValidator.Validate(suppliedResult);
                    validations.Add(validation);
                }
                catch (Exception e)
                {
                    Logger.Error($"ERROR! Validating supplied results");
                    Logger.Error(e.Message);
                    Logger.Error(e.StackTrace);

                    validations.Add(new TestCaseValidation
                    {
                        TestCaseId = caseValidator.TestCaseId,
                        Reason = "Unexpected failure",
                        Result = "failed"
                    });
                }

            }

            return new TestVectorValidation { Validations = validations };
        }
        private static Logger Logger
        {
            get { return LogManager.GetLogger("Generate"); }
        }
    }
}
