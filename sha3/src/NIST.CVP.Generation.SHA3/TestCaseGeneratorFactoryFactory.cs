using System;
using NIST.CVP.Generation.Core;
using System.Linq;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _iTestCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory)
        {
            _iTestCaseGeneratorFactory = iTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVectorSet)
        {
            var testId = 1;
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = _iTestCaseGeneratorFactory.GetCaseGenerator(group);

                for (var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; caseNo++)
                {
                    var testCaseResponse = generator.Generate(group, testVectorSet.IsSample);

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
