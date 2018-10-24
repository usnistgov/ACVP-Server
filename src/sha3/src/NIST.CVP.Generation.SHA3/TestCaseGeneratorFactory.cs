using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.SHA3
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
                return new TestCaseGeneratorAft(_oracle);
            }
            else if (testGroup.TestType.ToLower() == "mct")
            {
                if (testGroup.Function.ToLower() == "shake")
                {
                    return new TestCaseGeneratorShakeMct(_oracle);
                }
                else
                {
                    return new TestCaseGeneratorMct(_oracle);
                }
            }
            else if (testGroup.TestType.ToLower() == "vot")
            {
                return new TestCaseGeneratorVot(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
