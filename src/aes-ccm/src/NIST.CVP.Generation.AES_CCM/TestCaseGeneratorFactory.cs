using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_CCM _algo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_CCM algo)
        {
            _algo = algo;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
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
