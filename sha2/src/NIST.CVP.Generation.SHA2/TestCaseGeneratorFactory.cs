using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;
        private readonly ISHA_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA algo, ISHA_MCT mctAlgo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group, bool isSample)
        {
            if(group.TestType.ToLower() == "aft")
            {
                return new TestCaseGeneratorAFTHash(_random800_90, _algo);
            }
            else if(group.TestType.ToLower() == "mct")
            {
                return new TestCaseGeneratorMCTHash(_random800_90, _mctAlgo, isSample);
            }

            return new TestCaseGeneratorNull();
        }

        public GenerateResponse BuildTestCases(ITestVectorSet testVector)
        {
            var testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g).Where(w => w.TestType.ToLower() == "aft"))
            {
                // Get the generator for this test type
                var generator = GetCaseGenerator(group, testVector.IsSample);

                // Generate the first test case which allows us to know exactly how many test cases we need
                //var firstResponse = GenerateFirstCase(group, generator, testVector.IsSample, testId);
                //if (!firstResponse.Success)
                //{
                //    return new GenerateResponse(firstResponse.ErrorMessage);
                //}

                var responses = ((TestCaseGeneratorAFTHash) generator).GenerateInParallel(group, testVector.IsSample, testId);
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
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g).Where(w => w.TestType.ToLower() == "mct"))
            {
                var generator = GetCaseGenerator(group, testVector.IsSample);
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

            return new GenerateResponse();
        }

        private GenerateResponse GenerateFirstCase(TestGroup group, ITestCaseGenerator<TestGroup, TestCase> generator, bool isSample, int testId)
        {
            var testCaseResponse = generator.Generate(group, isSample);

            if (!testCaseResponse.Success)
            {
                return new GenerateResponse(testCaseResponse.ErrorMessage);
            }

            var testCase = (TestCase)testCaseResponse.TestCase;
            testCase.TestCaseId = testId;
            group.Tests.Add(testCase);

            return new GenerateResponse();
        }

        private void GenerateCase(TestGroup group, ITestCaseGenerator<TestGroup, TestCase> generator, bool isSample, int tsId)
        {
            // Run generation
            var testCaseResponse = generator.Generate(group, isSample);

            if (!testCaseResponse.Success)
            {
                throw new Exception(testCaseResponse.ErrorMessage);
                //return new GenerateResponse(testCaseResponse.ErrorMessage);
            }

            // Store results in group
            var testCase = (TestCase)testCaseResponse.TestCase;
            testCase.TestCaseId = tsId;
            group.Tests.Add(testCase);
        }
    }
}
