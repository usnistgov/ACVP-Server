using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorFactoryFactoryTdes : ITestCaseGeneratorFactoryFactory<TestVectorSetTdes>
    {
        private readonly ITestCaseGeneratorFactory<TestGroupTdes, TestCaseTdes> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactoryTdes(ITestCaseGeneratorFactory<TestGroupTdes, TestCaseTdes> testCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSetTdes testVectorSet)
        {
            int testId = 1;
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroupTdes)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(@group, testVectorSet.IsSample);
                    if (!testCaseResponse.Success)
                    {
                        return new GenerateResponse(testCaseResponse.ErrorMessage);
                    }
                    var testCase = (TestCaseTdes)testCaseResponse.TestCase;
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }
            }

            return new GenerateResponse();
        }
    }
}
