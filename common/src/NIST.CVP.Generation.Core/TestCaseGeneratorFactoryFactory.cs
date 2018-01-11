using System.Linq;

namespace NIST.CVP.Generation.Core
{
    public class TestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> : ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        private readonly ITestCaseGeneratorFactory<TTestGroup, TTestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TTestGroup, TTestCase> iTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TTestVectorSet testVector)
        {
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TTestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(@group, testVector.IsSample);
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
