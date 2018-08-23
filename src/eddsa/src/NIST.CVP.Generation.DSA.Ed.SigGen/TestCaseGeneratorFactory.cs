using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.Ed.SigGen
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
            if (testGroup.TestType.ToLower() == "aft")
            {
                return new TestCaseGenerator(_oracle);
            }
            else if (testGroup.TestType.ToLower() == "bft")
            {
                return new TestCaseGeneratorBft(_oracle);
            }
            else
            {
                return new TestCaseGeneratorNull();
            }
        }
    }
}
