using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var ivGen = testGroup.IVGeneration.ToLower();
            if (direction == "encrypt")
            {
                if (ivGen == "internal")
                {
                    return new TestCaseGeneratorInternalEncrypt(_oracle);
                }
                if (ivGen == "external")
                {
                    return new TestCaseGeneratorExternalEncrypt(_oracle);
                }
            }

            if (direction == "decrypt")
            {
                return new TestCaseGeneratorDecrypt(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
