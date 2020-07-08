using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
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
            return testGroup.TestType.ToLower() switch
            {
                "kat" => new TestCaseGeneratorKat(testGroup, _oracle),
                "aft" => new TestCaseGeneratorAft(_oracle),
                "gdt" => new TestCaseGeneratorGdt(_oracle),
                _ => new TestCaseGeneratorNull()
            };
        }
    }
}
