using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ICmacFactory _algoFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ICmacFactory algoFactory)
        {
            _algoFactory = algoFactory;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            ICmac cmac = _algoFactory.GetCmacInstance(testGroup.CmacType);
            
            if (direction == "gen")
            {
                return new TestCaseGeneratorGen(_random800_90, cmac);
            }

            if (direction == "ver")
            {
                return new TestCaseGeneratorVer(_random800_90, cmac);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
