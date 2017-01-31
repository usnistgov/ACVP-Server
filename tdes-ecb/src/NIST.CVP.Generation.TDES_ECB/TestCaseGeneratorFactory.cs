using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_ECB _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ITDES_ECB algo, IMonteCarloKeyMaker keyMaker)
        {
            _algo = algo;
            _random800_90 = random800_90;
            _keyMaker = keyMaker;
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
            if (@group.TestType.ToLower() == "montecarlo")
            {
                if (@group.Function.ToLower() == "encrypt")
                {
                    return new TestCaseGeneratorMonteCarloEncrypt(_random800_90,_algo, _keyMaker, isSample);
                }

                if (@group.Function.ToLower() == "decrypt")
                {
                    return new TestCaseGeneratorMonteCarloDecrypt(_random800_90, _algo, _keyMaker, isSample);
                }
            }
            return new TestCaseGeneratorNull();
        }
    }
}
