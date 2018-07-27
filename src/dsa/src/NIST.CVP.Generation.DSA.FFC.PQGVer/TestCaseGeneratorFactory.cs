using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
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
