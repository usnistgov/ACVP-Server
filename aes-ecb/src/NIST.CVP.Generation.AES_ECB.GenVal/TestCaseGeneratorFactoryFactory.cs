using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.GenVal
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase>  _staticTestCaseGeneratorFactory;
        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory, IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase> iStaticTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
            _staticTestCaseGeneratorFactory = iStaticTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVector)
        {
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g).Where(w => !w.StaticGroupOfTests))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (int caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = generator.Generate(@group, testVector.IsSample);
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
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g).Where(w => w.StaticGroupOfTests))
            {
                var generator = _staticTestCaseGeneratorFactory.GetStaticCaseGenerator(group);
                var tests = generator.Generate(group);
                if (!tests.Success)
                {
                    return new GenerateResponse(tests.ErrorMessage);
                }
                foreach (var testCase in tests.TestCases)
                {
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }
            }

            return new GenerateResponse();
        }
    }
}
