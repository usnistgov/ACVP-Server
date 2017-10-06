using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVectorSet)
        {
            int testId = 1;
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(@group, testVectorSet.IsSample);
                    if (!testCaseResponse.Success)
                    {
                        return new GenerateResponse(testCaseResponse.ErrorMessage);
                    }
                    var testCase = (TestCase)testCaseResponse.TestCase;
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }
            }

            return new GenerateResponse();
        }
    }
}