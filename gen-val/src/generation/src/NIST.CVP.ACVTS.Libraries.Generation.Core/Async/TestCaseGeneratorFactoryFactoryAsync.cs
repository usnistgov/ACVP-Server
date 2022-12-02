using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NIST.CVP.ACVTS.Libraries.Common.Config;
using NIST.CVP.ACVTS.Libraries.Common.Enums;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Async
{
    public class TestCaseGeneratorFactoryFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private readonly ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> _testCaseGeneratorFactory;
        private readonly int _maximumWorkToQueue;

        public TestCaseGeneratorFactoryFactoryAsync(
            ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> iTestCaseGeneratorFactory,
            IOptions<OrleansConfig> orleansConfig)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
            _maximumWorkToQueue = orleansConfig.Value.MaxWorkItemsToQueuePerGenValInstance;
        }

        public async Task<GenerateResponse> BuildTestCasesAsync(TTestVectorSet testVector)
        {
            var semaphore = new SemaphoreSlim(_maximumWorkToQueue, _maximumWorkToQueue);

            // Keep track of a map of tasks for generating test cases, keyed on the group
            var map = new Dictionary<TTestGroup, List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>>();
            var allTasks = new List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>();

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
                map.Add(group, groupTasks);

                for (var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    await semaphore.WaitAsync();
                    var caseNumberClosure = caseNo;
                    groupTasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            return generator.GenerateAsync(@group, testVector.IsSample, caseNumberClosure);
                        }
                        catch (Exception e)
                        {
                            _logger.Error(e, e.Message);
                            throw;
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                    allTasks.AddRange(groupTasks);
                }
            }

            await Task.WhenAll(allTasks);
            int testId = 1;

            // for each group
            foreach (var keyValuePair in map)
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

        private static ILogger _logger = LogManager.GetCurrentClassLogger();
    }
}
