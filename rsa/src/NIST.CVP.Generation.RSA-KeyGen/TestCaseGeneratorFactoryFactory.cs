using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private const string KAT_TAG = "kat";

        private readonly ITestCaseGeneratorFactory<TestGroup, TestCase> _testCaseGeneratorFactory;
        private readonly IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase> _staticTestCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory<TestGroup, TestCase> iTestCaseGeneratorFactory,
            IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase> iStaticTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
            _staticTestCaseGeneratorFactory = iStaticTestCaseGeneratorFactory;
        }

        public GenerateResponse BuildTestCases(TestVectorSet testVector)
        {
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup) g).Where(w => w.TestType.ToLower() != KAT_TAG))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group);
                for (var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; ++caseNo)
                {
                    var testCaseResponse = new TestCaseGenerateResponse("First");
                    do
                    {
                        testCaseResponse = generator.Generate(group, testVector.IsSample);

                    // Loop til success for AFTs
                    } while (!testCaseResponse.Success);

                    if (!testCaseResponse.Success)
                    {
                        return new GenerateResponse(testCaseResponse.ErrorMessage);
                    }

                    var testCase = (TestCase) testCaseResponse.TestCase;
                    testCase.TestCaseId = testId;
                    group.Tests.Add(testCase);
                    testId++;
                }
            }

            foreach (var group in testVector.TestGroups.Select(g => (TestGroup) g).Where(w => w.TestType.ToLower() == KAT_TAG))
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
