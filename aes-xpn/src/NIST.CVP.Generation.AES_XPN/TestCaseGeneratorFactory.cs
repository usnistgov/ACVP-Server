using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XPN
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_GCM _aesGcm;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_GCM aesGcm)
        {
            _aesGcm = aesGcm;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var ivGen = testGroup.IVGeneration.ToLower();
            var saltGen = testGroup.SaltGen.ToLower();
            if (direction == "encrypt")
            {
                if (ivGen == "external" && saltGen == "external")
                {
                    return new TestCaseGeneratorExternalEncrypt(_random800_90, _aesGcm);
                }

                if (ivGen == "internal" || saltGen == "internal")
                {
                    return new TestCaseGeneratorInternalEncrypt(_random800_90, _aesGcm);
                }
            }

            if (direction == "decrypt")
            {
                return new TestCaseGeneratorDecrypt(_random800_90, _aesGcm);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
