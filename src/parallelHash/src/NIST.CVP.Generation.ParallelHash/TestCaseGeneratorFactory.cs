using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ParallelHash
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
            else if (testGroup.TestType.ToLower() == "mct")
            {
                return new TestCaseGeneratorMct(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
