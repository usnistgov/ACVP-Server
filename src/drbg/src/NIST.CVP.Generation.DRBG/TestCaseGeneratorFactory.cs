using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
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
            if (testGroup.ReSeed)
            {
                if (testGroup.PredResistance)
                {
                    return new TestCaseGeneratorReseedPredResist(_oracle);
                }
                
                return new TestCaseGeneratorReseedNoPredResist(_oracle);
            }

            return new TestCaseGeneratorNoReseed(_oracle);
        }
    }
}
