using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;

namespace NIST.CVP.Generation.ECDSA.Fips186_5.DetSigGen
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
            return new TestCaseGenerator(_oracle);
        }
    }
}