using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.SHA2
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
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g).Where(w => w.TestType.ToLower() == "aft"))
            {
                // Get the generator for this test type
                var generator = _iTestCaseGeneratorFactory.GetCaseGenerator(group);
                var responses = ((TestCaseGeneratorAFTHash)generator).GenerateInParallel(group, testVectorSet.IsSample, testId);
                foreach (var response in responses)
                {
                    if (!response.Success)
                    {
                        return new GenerateResponse(response.ErrorMessage);
                    }

                    group.Tests.Add(response.TestCase);
                }

                testId += generator.NumberOfTestCasesToGenerate;
                group.Tests.Sort((x, y) => x.TestCaseId.CompareTo(y.TestCaseId));
            }

            // MCT stuff can't be parallelized, just let it run alone
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g).Where(w => w.TestType.ToLower() == "mct"))
            {
                var generator = _iTestCaseGeneratorFactory.GetCaseGenerator(group);
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

            return new GenerateResponse();
        }
    }
}
