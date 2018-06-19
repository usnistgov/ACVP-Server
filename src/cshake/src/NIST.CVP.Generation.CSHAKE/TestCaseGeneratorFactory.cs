using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ICSHAKE _algo;
        private readonly ICSHAKE_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ICSHAKE algo, ICSHAKE_MCT mctAlgo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _mctAlgo = mctAlgo;
        }
        
        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.ToLower() == "aft")
            {
                return new TestCaseGeneratorAFTHash(_random800_90, _algo);
            }
            else if (testGroup.TestType.ToLower() == "mct")
            {
                return new TestCaseGeneratorMCTHash(_random800_90, _mctAlgo);
            }
            else if (testGroup.TestType.ToLower() == "vot")
            {
                    return new TestCaseGeneratorVOTHash(_random800_90, _algo);
            }
            
            return new TestCaseGeneratorNull();
        }
    }
}
