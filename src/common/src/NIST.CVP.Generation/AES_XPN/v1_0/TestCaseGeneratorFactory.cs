using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_XPN.v1_0
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
            var saltGen = testGroup.SaltGen.ToLower();
            if (direction == "encrypt")
            {
                if (ivGen == "external" && saltGen == "external")
                {
                    return new TestCaseGeneratorExternalEncrypt(_oracle);
                }

                if (ivGen == "internal" || saltGen == "internal")
                {
                    return new TestCaseGeneratorInternalEncrypt(_oracle);
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
