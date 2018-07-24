using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Enums;
using NLog;

namespace NIST.CVP.Generation.Core.Async
{
    public class ResultValidatorAsync<TTestGroup, TTestCase> : IResultValidatorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public TestVectorValidation ValidateResults(
            IEnumerable<ITestCaseValidatorAsync<TTestGroup, TTestCase>> testCaseValidators, 
            IEnumerable<TTestGroup> testResults,
            bool showExpected
        )
        {
            var task = ValidateResultsAsync(testCaseValidators, testResults, showExpected);
            task.Wait();
            return task.Result;
        }

        private async Task<TestVectorValidation> ValidateResultsAsync(
            IEnumerable<ITestCaseValidatorAsync<TTestGroup, TTestCase>> testCaseValidators, 
            IEnumerable<TTestGroup> testResults, 
            bool showExpected
        )
        {
            // Keep track of validation tasks
            var tasks = new List<Task<TestCaseValidation>>();

            // result of validations
            var validations = new List<TestCaseValidation>();

            // for every test case validator, start a task to validate the test
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
                    tasks.Add(caseValidator.ValidateAsync(suppliedResult, showExpected));
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

            validations.AddRange(await Task.WhenAll(tasks));

            return new TestVectorValidation { Validations = validations };
        }

        private static Logger Logger => LogManager.GetLogger("Generate");
    }
}