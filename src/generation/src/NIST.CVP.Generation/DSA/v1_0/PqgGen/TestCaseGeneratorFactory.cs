using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.v1_0.PqgGen
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
            if (testGroup.PQGenMode != PrimeGenMode.None)
            {
                return new TestCaseGeneratorPQ(_oracle);
            }

            if (testGroup.GGenMode != GeneratorGenMode.None)
            {
                return new TestCaseGeneratorG(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
