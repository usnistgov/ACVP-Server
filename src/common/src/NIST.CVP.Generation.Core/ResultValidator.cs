using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core.Enums;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class ResultValidator<TTestGroup, TTestCase> : IResultValidator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public TestVectorValidation ValidateResults(
            IEnumerable<ITestCaseValidator<TTestGroup, TTestCase>> testCaseValidators, 
            IEnumerable<TTestGroup> testResults,
            bool showExpected
        )
        {
            var validations = new List<TestCaseValidation>();
            foreach (var caseValidator in testCaseValidators)
            {
                var suppliedResult = testResults.SelectMany(tg => tg.Tests).FirstOrDefault(tc => tc.TestCaseId == caseValidator.TestCaseId);
                if (suppliedResult == null)
                {
                    validations.Add(new TestCaseValidation
                    {
                        TestCaseId = caseValidator.TestCaseId,
                        Result = Disposition.Missing
                    });
                    continue;
                }

                try
                {
                    var validation = caseValidator.Validate(suppliedResult, showExpected);
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
