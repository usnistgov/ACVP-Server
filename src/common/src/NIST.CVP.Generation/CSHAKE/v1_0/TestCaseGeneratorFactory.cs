using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CSHAKE.v1_0
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly IRandom800_90 _rand;

        public TestCaseGeneratorFactory(IOracle oracle, IRandom800_90 rand)
        {
            _oracle = oracle;
            _rand = rand;
        }
        
        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.ToLower() == "aft")
            {
                return new TestCaseGeneratorAft(_oracle, _rand);
            }
            else if (testGroup.TestType.ToLower() == "mct")
            {
                return new TestCaseGeneratorMct(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
