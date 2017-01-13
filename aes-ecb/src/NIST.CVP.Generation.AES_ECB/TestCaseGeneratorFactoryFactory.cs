using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorFactoryFactory : ITestCaseGeneratorFactoryFactory<TestVectorSet>
    {
        private readonly IStaticTestCaseGeneratorFactory<TestGroup, IEnumerable<TestCase>>  _staticTestCaseGeneratorFactory;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;

        public TestCaseGeneratorFactoryFactory(ITestCaseGeneratorFactory iTestCaseGeneratorFactory, IStaticTestCaseGeneratorFactory<TestGroup, IEnumerable<TestCase>> iStaticTestCaseGeneratorFactory)
        {
            _testCaseGeneratorFactory = iTestCaseGeneratorFactory;
            _staticTestCaseGeneratorFactory = iStaticTestCaseGeneratorFactory;
        }

        public int NumberOfCases { get { return 15; } }

        public GenerateResponse BuildTestCases(TestVectorSet testVector)
        {
            int testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g).Where(w => !w.StaticGroupOfTests))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group.Function, group.TestType);
                for (int caseNo = 0; caseNo < NumberOfCases; ++caseNo)
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
                var generator = _staticTestCaseGeneratorFactory.GetStaticCaseGenerator(group.Function, group.TestType);
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
