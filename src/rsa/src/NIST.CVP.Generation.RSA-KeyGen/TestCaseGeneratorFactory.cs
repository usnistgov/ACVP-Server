using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IKeyComposerFactory _keyComposerFactory;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
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
