using System;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA _algo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA algo)
        {
            _random800_90 = random800_90;
            _algo = algo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group, bool isSample)
        {
            if(group.TestType.ToLower() == "longmessage")
            {
                return new TestCaseGeneratorLongHash(_random800_90, _algo);
            }
            else if(group.TestType.ToLower() == "shortmessage")
            {
                return new TestCaseGeneratorShortHash(_random800_90, _algo);
            }
            else if(group.TestType.ToLower() == "montecarlo")
            {
                return new TestCaseGeneratorMonteCarloHash(_random800_90, _algo, isSample);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
