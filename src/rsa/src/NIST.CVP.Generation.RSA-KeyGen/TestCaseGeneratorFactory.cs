using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA_KeyGen
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
            switch (testGroup.TestType.ToLower())
            {
                case "kat":
                    return new TestCaseGeneratorKat(testGroup, _oracle);

                case "aft":
                case "gdt":
                    // Aft and Gdt generator would do the same function (validators differ) so they are lumped together
                    return new TestCaseGeneratorAft(_oracle);

                default:
                    return new TestCaseGeneratorNull();
            }
        }
    }
}
