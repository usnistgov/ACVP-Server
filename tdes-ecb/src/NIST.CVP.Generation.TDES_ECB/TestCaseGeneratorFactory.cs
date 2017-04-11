using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_ECB _algo;
        private readonly ITDES_ECB_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ITDES_ECB algo, ITDES_ECB_MCT mctAlgo)
        {
            _algo = algo;
            _random800_90 = random800_90;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup @group, bool isSample)
        {

            if (@group.TestType.ToLower() == "multiblockmessage")
            {
                if (@group.Function.ToLower() == "encrypt")
                {
                    return new TestCaseGeneratorMMTEncrypt(_random800_90, _algo);
                }

                if (@group.Function.ToLower() == "decrypt")
                {
                    return new TestCaseGeneratorMMTDecrypt(_random800_90, _algo);
                }
            }
            if (@group.TestType.ToLower() == "mct")
            {
                if (@group.Function.ToLower() == "encrypt")
                {
                    return new TestCaseGeneratorMonteCarloEncrypt(_random800_90, _mctAlgo);
                }

                if (@group.Function.ToLower() == "decrypt")
                {
                    return new TestCaseGeneratorMonteCarloDecrypt(_random800_90, _mctAlgo);
                }
            }
            return new TestCaseGeneratorNull();
        }
    }
}
