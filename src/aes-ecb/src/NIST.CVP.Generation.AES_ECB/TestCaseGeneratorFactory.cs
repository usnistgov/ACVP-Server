using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_ECB _algo;
        private readonly IAES_ECB_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_ECB algo, IAES_ECB_MCT mctAlgo)
        {
            _algo = algo;
            _random800_90 = random800_90;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var testType = testGroup.TestType.ToLower();

            switch (testType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(testGroup.KeyLength, testType);
                case "mct":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMCTEncrypt(_random800_90, _mctAlgo);
                        case "decrypt":
                            return new TestCaseGeneratorMCTDecrypt(_random800_90, _mctAlgo);
                    }
                    break;
                case "mmt":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _algo);
                    }
                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
