using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_GCM.v1_0
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var direction = testGroup.Function.ToLower();
            var ivGen = testGroup.IvGeneration.ToLower();
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
