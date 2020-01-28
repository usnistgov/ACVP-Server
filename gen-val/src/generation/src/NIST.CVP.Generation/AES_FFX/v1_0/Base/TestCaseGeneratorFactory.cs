using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.AES_FFX.v1_0.Base
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _random;

        public TestCaseGeneratorFactory(IOracle oracle, IRandom800_90 random)
        {
            _oracle = oracle;
            _random = random;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            return new TestCaseGenerator(_oracle, _random);
        }
    }
}
