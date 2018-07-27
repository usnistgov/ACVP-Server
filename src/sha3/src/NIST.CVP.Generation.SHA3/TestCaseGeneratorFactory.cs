using NIST.CVP.Crypto.Common.Hash.SHA3;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISHA3 _algo;
        private readonly ISHA3_MCT _mctAlgo;
        private readonly ISHAKE_MCT _shakeMctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISHA3 algo, ISHA3_MCT mctAlgo, ISHAKE_MCT shakeMctAlgo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _mctAlgo = mctAlgo;
            _shakeMctAlgo = shakeMctAlgo;
        }
        
        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.Function.ToLower() == "sha3")
            {
                if (testGroup.TestType.ToLower() == "aft")
                {
                    return new TestCaseGeneratorSHA3AFTHash(_random800_90, _algo);
                }
                else if (testGroup.TestType.ToLower() == "mct")
                {
                    return new TestCaseGeneratorSHA3MCTHash(_random800_90, _mctAlgo);
                }
            }
            else if (testGroup.Function.ToLower() == "shake")
            {
                if (testGroup.TestType.ToLower() == "aft")
                {
                    return new TestCaseGeneratorSHAKEAFTHash(_random800_90, _algo);
                }
                else if (testGroup.TestType.ToLower() == "mct")
                {
                    return new TestCaseGeneratorSHAKEMCTHash(_random800_90, _shakeMctAlgo);
                }
                else if (testGroup.TestType.ToLower() == "vot")
                {
                    return new TestCaseGeneratorSHAKEVOTHash(_random800_90, _algo);
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}
