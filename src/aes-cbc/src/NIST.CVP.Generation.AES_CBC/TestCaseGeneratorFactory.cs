using NIST.CVP.Crypto.AES_CBC;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CBC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_CBC _aesCbc;
        private readonly IAES_CBC_MCT _aesCbcMct;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_CBC aesCbc, IAES_CBC_MCT aesCbcMct)
        {
            _random800_90 = random800_90;
            _aesCbc = aesCbc;
            _aesCbcMct = aesCbcMct;
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
                            return new TestCaseGeneratorMCTEncrypt(_random800_90, _aesCbcMct);
                        case "decrypt":
                            return new TestCaseGeneratorMCTDecrypt(_random800_90, _aesCbcMct);
                    }
                    break;
                case "mmt":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _aesCbc);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _aesCbc);
                    }
                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
