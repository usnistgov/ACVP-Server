using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_ECB _algo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ITDES_ECB algo)
        {
            _algo = algo;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(string direction)
        {
            direction = direction.ToLower();
            if (direction == "encrypt")
            {
                return new TestCaseGeneratorEncrypt(_random800_90, _algo);
            }

            if (direction == "decrypt")
            {
                return new TestCaseGeneratorDecrypt(_random800_90, _algo);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
