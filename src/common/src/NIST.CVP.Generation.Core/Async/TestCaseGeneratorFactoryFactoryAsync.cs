using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core.Async
{
    public class TestCaseGeneratorFactoryFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private readonly ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactoryAsync(ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> iTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TTestVectorSet testVector)
        {
            var task = BuildTestCasesAsync(testVector);
            task.Wait();
            return task.Result;
        }

        private async Task<GenerateResponse> BuildTestCasesAsync(TTestVectorSet testVector)
        {
            // Keep track of a map of tasks for generating test cases, keyed on the group
            var tasks = new Dictionary<
                TTestGroup, List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>
            >();

            // for every test group, get a generator,
            // and start tasks attibuted to the group for creating test cases
            foreach (var group in testVector.TestGroups.Select(g => g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                var groupTasks = new List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>();
                tasks.Add(group, groupTasks);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    groupTasks.Add(generator.GenerateAsync(group, testVector.IsSample));
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
    }
}
