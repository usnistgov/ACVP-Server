using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Enums;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class ResultValidator<TTestGroup, TTestCase> : IResultValidator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        public TestVectorValidation ValidateResults(IEnumerable<ITestCaseValidator<TTestCase>> testCaseValidators, IEnumerable<TTestGroup> testResults)
        {
            var validations = new List<TestCaseValidation>();
            foreach (var caseValidator in testCaseValidators)
            {
                // TODO avoid cast here?
                var suppliedResult = (TTestCase) testResults.SelectMany(tg => tg.Tests).FirstOrDefault(tc => tc.TestCaseId == caseValidator.TestCaseId);
                if (suppliedResult == null)
                {
                    validations.Add(new TestCaseValidation {TestCaseId = caseValidator.TestCaseId, Result = Disposition.Missing});
                    continue;
                }

                try
                {
                    var validation = caseValidator.Validate(suppliedResult);
                    validations.Add(validation);
                }
                catch (Exception e)
                {
                    Logger.Error("ERROR! Validating supplied results");
                    Logger.Error(e.Message);
                    Logger.Error(e.StackTrace);

                    validations.Add(new TestCaseValidation
                    {
                        TestCaseId = caseValidator.TestCaseId,
                        Reason = "Unexpected failure",
                        Result = Disposition.Failed
                    });
                }
            }

            return new TestVectorValidation { Validations = validations };
        }

        private static Logger Logger => LogManager.GetLogger("Generate");
    }
}
