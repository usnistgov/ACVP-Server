using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase> _staticTestCaseGeneratorFactory;
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory, IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase> iStaticTestCaseGeneratorFactory = null)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
            
            // See if this is needed... No static tests for SHA1
            _staticTestCaseGeneratorFactory = iStaticTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVector)
        {
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(group, testVector.IsSample);
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
