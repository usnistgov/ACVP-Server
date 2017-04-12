using NIST.CVP.Crypto.AES_OFB;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_OFB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_OFB _aesOfb;
        private readonly IAES_OFB_MCT _aesOfbMct;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_OFB aesOfb, IAES_OFB_MCT aesOfbMct)
        {
            _random800_90 = random800_90;
            _aesOfb = aesOfb;
            _aesOfbMct = aesOfbMct;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var testType = testGroup.TestType.ToLower();

            if (testType == "mct")
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorMCTEncrypt(_random800_90, _aesOfbMct);
                }

                if (direction == "decrypt")
                {
                    return new TestCaseGeneratorMCTDecrypt(_random800_90, _aesOfbMct);
                }
            }
            else
            {
                if (direction == "encrypt")
                {
                    return new TestCaseGeneratorMMTEncrypt(_random800_90, _aesOfb);
                }

                if (direction == "decrypt")
                {
                    return new TestCaseGeneratorMMTDecrypt(_random800_90, _aesOfb);
                }
            }
            return new TestCaseGeneratorNull();
        }
    }
}
