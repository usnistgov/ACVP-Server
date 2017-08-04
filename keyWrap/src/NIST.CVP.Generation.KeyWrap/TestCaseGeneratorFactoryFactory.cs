using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseGeneratorFactoryFactory<TTestVectorSet>
        where TTestVectorSet : TestVectorSetBase<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestCase>, new()
        where TTestCase : TestCaseBase, new()
    {
        private readonly ITestCaseGeneratorFactory<TTestGroup, TTestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TTestGroup, TTestCase> testCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TTestVectorSet testVectorSet)
        {
            int testId = 1;
            foreach (var group in testVectorSet.TestGroups.Select(g => (TTestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(@group, testVectorSet.IsSample);
                    if (!testCaseResponse.Success)
                    {
                        return new GenerateResponse(testCaseResponse.ErrorMessage);
                    }
                    var testCase = (TTestCase)testCaseResponse.TestCase;
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }
            }

            return new GenerateResponse();
        }
    }
}
