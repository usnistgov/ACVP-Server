using NIST.CVP.Crypto.AES_CFB128;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB128
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_CFB128 _algo;
        private readonly IAES_CFB128_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_CFB128 algo, IAES_CFB128_MCT mctAlgo)
        {
            _random800_90 = random800_90;
            _algo = algo;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var testType = testGroup.TestType.ToLower();

            if (testType == "mct")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorMCTEncrypt(_random800_90, _mctAlgo);
                }

                if (direction == "decrypt")
                {
                    return new TestCaseGeneratorMCTDecrypt(_random800_90, _mctAlgo);
                }
            }
            else
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorMMTEncrypt(_random800_90, _algo);
                }

                if (direction == "decrypt")
                {
                    return new TestCaseGeneratorMMTDecrypt(_random800_90, _algo);
                }
            }
            return new TestCaseGeneratorNull();
        }
    }
}
