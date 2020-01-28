using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA.v1_0.DpComponent
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            return new TestCaseGenerator(_oracle);
        }
    }
}
