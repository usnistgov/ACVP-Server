using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3 _algo;
        private readonly ISHA3_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA3 algo, ISHA3_MCT mctAlgo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group, bool isSample)
        {
            if (group.Function.ToLower() == "sha3")
            {
                if (group.TestType.ToLower() == "aft")
                {
                    return new TestCaseGeneratorSHA3AFTHash(_random800_90, _algo);
                }
                else if (group.TestType.ToLower() == "mct")
                {
                    return new TestCaseGeneratorSHA3MCTHash(_random800_90, _mctAlgo, isSample);
                }
            }
            else if (group.Function.ToLower() == "shake")
            {
                if (group.TestType.ToLower() == "aft")
                {
                    return new TestCaseGeneratorSHAKEAFTHash(_random800_90, _algo);
                }
                else if (group.TestType.ToLower() == "mct")
                {
                    return new TestCaseGeneratorSHAKEMCTHash(_random800_90, _mctAlgo, isSample);
                }
                else if (group.TestType.ToLower() == "vot")
                {
                    return null;
                }
            }

            return new TestCaseGeneratorNull();
        }

        public GenerateResponse BuildTestCases(ITestVectorSet testVector)
        {
            var testId = 1;
            foreach (var group in testVector.TestGroups.Select(g => (TestGroup) g))
            {
                var generator = GetCaseGenerator(group, testVector.IsSample);

                for (var caseNo = 0; caseNo < generator.NumberOfTestCasesToGenerate; caseNo++)
                {
                    var testCaseResponse = generator.Generate(group, testVector.IsSample);

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

            return new GenerateResponse();
        }
    }
}
