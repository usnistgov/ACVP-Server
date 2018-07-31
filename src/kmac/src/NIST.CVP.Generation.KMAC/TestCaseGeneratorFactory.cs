using NIST.CVP.Generation.Core;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Generation.KMAC
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
            if (testGroup.TestType.ToLower() == "aft")
            {
                return new TestCaseGeneratorAft(_oracle);
            }
            else if (testGroup.TestType.ToLower() == "mvt")
            {
                return new TestCaseGeneratorMvt(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
