using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Enums;
using NIST.CVP.Common.ExtensionMethods;
using NLog;

namespace NIST.CVP.Generation.Core.Async
{
    public class TestCaseGeneratorFactoryFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private const int ThrottleDelayInMilliseconds = 1000;

        private readonly ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> _testCaseGeneratorFactory;
        private readonly int _maximumWorkToQueue;
        private readonly object _lockCounter = new object();

        private int _queuedWorkCounter;
        private int _totalWorkQueued;
        
        public TestCaseGeneratorFactoryFactoryAsync(
            ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> iTestCaseGeneratorFactory,
            IOptions<OrleansConfig> orleansConfig)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
            _maximumWorkToQueue = orleansConfig.Value.MaxWorkItemsToQueuePerGenValInstance;
        }

        public async Task<GenerateResponse> BuildTestCasesAsync(TTestVectorSet testVector)
        {
            // Keep track of a map of tasks for generating test cases, keyed on the group
            var tasks = new Dictionary<
                TTestGroup, List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>
            >();
            
            // for every test group, get a generator,
            // and start tasks attributed to the group for creating test cases
            foreach (var group in testVector.TestGroups.Select(g => g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                if (generator is ITestCaseGeneratorWithPrep<TTestGroup, TTestCase> genWithPrep)
                {
                    var response = genWithPrep.PrepareGenerator(group, testVector.IsSample);
                    if (!response.Success)
                    {
                        return new GenerateResponse(response.ErrorMessage, StatusCode.TestCaseGeneratorError);
                    }
                }
                
                var groupTasks = new List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>();
                tasks.Add(group, groupTasks);
                
                for (var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    QueueWork(testVector, groupTasks, generator, @group, caseNo).FireAndForget();
                }
                
                while (groupTasks.Count != generator.NumberOfTestCasesToGenerate)
                {
                    await Task.Delay(ThrottleDelayInMilliseconds);
                }
            }
            
            int testId = 1;

            // for each group
            foreach (var keyValuePair in tasks)
            {
                var group = keyValuePair.Key;
                // foreach task in the group, once they're completed, finish setting up the test case
                foreach (var task in await Task.WhenAll(keyValuePair.Value))
                {
                    if (!task.Success)
                    {
                        return new GenerateResponse(task.ErrorMessage, StatusCode.TestCaseGeneratorError);
                    }
                    var testCase = task.TestCase;
                    testCase.ParentGroup = group;
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }
            }

            return new GenerateResponse();
        }

        private async Task QueueWork(TTestVectorSet testVector, List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>> groupTasks, ITestCaseGeneratorAsync<TTestGroup, TTestCase> generator,
            TTestGroup @group, int caseNo)
        {
            var debugLogged = false;
            
            while (true)
            {
                if (TryLock())
                {
                    var task = generator.GenerateAsync(@group, testVector.IsSample, caseNo);
                    groupTasks.Add(task);

                    await task;

                    lock (_lockCounter)
                    {
                        _queuedWorkCounter--;
                        _logger.Debug($"Generation Task has completed.  Currently {_queuedWorkCounter} of {_maximumWorkToQueue} tasks are queued.");
                        return;
                    }
                }

                // Only show this message once
                if (!debugLogged)
                {
                    _logger.Debug($"No additional work can be queued, trying again.");
                    debugLogged = true;
                }
                
                await Task.Delay(ThrottleDelayInMilliseconds);
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
                    _logger.Debug($"Enqueueing generation task {_totalWorkQueued}.  Currently {_queuedWorkCounter} of {_maximumWorkToQueue} tasks are queued.");

                    return true;
                }

                return false;
            }
        }
        
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
    }
}
