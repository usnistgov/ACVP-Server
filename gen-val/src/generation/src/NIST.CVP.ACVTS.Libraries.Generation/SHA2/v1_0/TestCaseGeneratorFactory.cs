using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0
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
            switch (testGroup.TestType.ToLower())
            {
                case "aft":
                    return new TestCaseGeneratorAFTHash(_oracle);

                case "mct":
                    return new TestCaseGeneratorMCTHash(_oracle);

                case "ldt":
                    return new TestCaseGeneratorLDTHash(_oracle);

                default:
                    return new TestCaseGeneratorNull();
            }
        }
    }
}
