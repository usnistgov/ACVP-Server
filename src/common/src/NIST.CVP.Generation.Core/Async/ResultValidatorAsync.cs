using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core.Enums;
using NLog;

namespace NIST.CVP.Generation.Core.Async
{
    public class ResultValidatorAsync<TTestGroup, TTestCase> : IResultValidatorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private const int ThrottleDelayInMilliseconds = 1000;

        private readonly int _maximumWorkToQueue;
        private readonly object _lockCounter = new object();

        private int _queuedWorkCounter;
        private int _totalWorkQueued;

        public ResultValidatorAsync(IOptions<OrleansConfig> orleansConfig)
        {
            _maximumWorkToQueue = orleansConfig.Value.MaxWorkItemsToQueuePerGenValInstance;
        }
    
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

            var expectedTaskCount = testCaseValidators.Count();
            
            // for every test case validator, start a task to validate the test
            foreach (var caseValidator in testCaseValidators)
            {
                var suppliedResult = testResults.SelectMany(tg => tg.Tests).FirstOrDefault(tc => tc.TestCaseId == caseValidator.TestCaseId);
                if (suppliedResult == null)
                {
                    expectedTaskCount--;
                    validations.Add(new TestCaseValidation
                    {
                        TestCaseId = caseValidator.TestCaseId,
                        Result = Disposition.Missing
                    });
                    continue;
                }

                try
                {
                    QueueWork(showExpected, tasks, caseValidator, suppliedResult).FireAndForget();
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
                }
            }

            while (tasks.Count != expectedTaskCount)
            {
                await Task.Delay(ThrottleDelayInMilliseconds);
            }
            
            validations.AddRange(await Task.WhenAll(tasks));

            return new TestVectorValidation { Validations = validations };
        }

        private async Task QueueWork(bool showExpected, List<Task<TestCaseValidation>> tasks, ITestCaseValidatorAsync<TTestGroup, TTestCase> caseValidator, TTestCase suppliedResult)
        {
            while (true)
            {
                if (TryLock())
                {
                    var task = caseValidator.ValidateAsync(suppliedResult, showExpected);
                    tasks.Add(task);

                    await task;

                    lock (_lockCounter)
                    {
                        _queuedWorkCounter--;
                        _logger.Debug($"Validation Task has completed.  Currently {_queuedWorkCounter} of {_maximumWorkToQueue} tasks are queued.");
                        return;
                    }
                }
                
                _logger.Debug($"No additional work can be queued, trying again.");
            }
        }
        
        private bool TryLock()
        {
            lock (_lockCounter)
            {
                if (_queuedWorkCounter < _maximumWorkToQueue)
                {
                    _queuedWorkCounter++;
                    _totalWorkQueued++;
                    _logger.Debug($"Enqueueing validation task {_totalWorkQueued}.  Currently {_queuedWorkCounter} of {_maximumWorkToQueue} tasks are queued.");

                    return true;
                }

                return false;
            }
        }

        private static Logger _logger => LogManager.GetCurrentClassLogger();
    }
}