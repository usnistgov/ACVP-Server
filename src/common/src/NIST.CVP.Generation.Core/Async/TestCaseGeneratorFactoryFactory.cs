using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Enums;

namespace NIST.CVP.Generation.Core.Async
{
    public class TestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private readonly ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactoryAsync<TTestGroup, TTestCase> iTestCaseGeneratorFactory)
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
            var tasks = new Dictionary<
                TTestGroup, List<Task<TestCaseGenerateResponse<TTestGroup, TTestCase>>>
            >();
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
            foreach (var keyValuePair in tasks)
            {
                var group = keyValuePair.Key;
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
