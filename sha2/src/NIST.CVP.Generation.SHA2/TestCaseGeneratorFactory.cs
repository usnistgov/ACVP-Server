using System.Linq;
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
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = GetCaseGenerator(group, testVector.IsSample);

                for (var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; caseNo++)
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
