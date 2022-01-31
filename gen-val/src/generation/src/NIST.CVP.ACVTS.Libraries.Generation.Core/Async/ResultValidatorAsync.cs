using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Async
{
    public class ResultValidatorAsync<TTestGroup, TTestCase> : IResultValidatorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private readonly int _maximumWorkToQueue;

        public ResultValidatorAsync(IOptions<OrleansConfig> orleansConfig)
        {
            _maximumWorkToQueue = orleansConfig.Value.MaxWorkItemsToQueuePerGenValInstance;
        }

        public async Task<TestVectorValidation> ValidateResultsAsync(
            List<ITestCaseValidatorAsync<TTestGroup, TTestCase>> testCaseValidators,
            List<TTestGroup> testResults,
            bool showExpected
        )
        {
            var semaphore = new SemaphoreSlim(_maximumWorkToQueue, _maximumWorkToQueue);

            // Keep track of validation tasks
            var validationTasks = new List<Task<TestCaseValidation>>();

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

                await semaphore.WaitAsync();
                validationTasks.Add(Task.Run(() =>
                {
                    try
                    {
                        return caseValidator.ValidateAsync(suppliedResult, showExpected);
                    }
                    catch (Exception e)
                    {
                        _logger.Error("ERROR! Validating supplied results");
                        _logger.Error(e.Message);
                        _logger.Error(e.StackTrace);

                        validations.Add(new TestCaseValidation
                        {
                            TestCaseId = caseValidator.TestCaseId,
                            Reason = "Unexpected failure",
                            Result = Disposition.Failed
                        });

                        return null;
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }

            var results = await Task.WhenAll(validationTasks.Where(w => w != null));

            validations.AddRange(results);

            return new TestVectorValidation { Validations = validations };
        }

        private static Logger _logger = LogManager.GetCurrentClassLogger();
    }
}
