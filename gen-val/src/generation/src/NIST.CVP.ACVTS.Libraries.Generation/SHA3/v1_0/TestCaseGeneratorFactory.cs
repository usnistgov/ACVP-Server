using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0
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
                if (testGroup.Function == ModeValues.SHAKE)
                {
                    return new TestCaseGeneratorShakeMct(_oracle);
                }
                else if (testGroup.Function == ModeValues.SHA3)
                {
                    return new TestCaseGeneratorMct(_oracle);
                }
            }
            else if (testGroup.TestType.ToLower() == "vot")
            {
                return new TestCaseGeneratorVot(_oracle);
            }
            else if (testGroup.TestType.ToLower() == "ldt")
            {
                return new TestCaseGeneratorLdt(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
