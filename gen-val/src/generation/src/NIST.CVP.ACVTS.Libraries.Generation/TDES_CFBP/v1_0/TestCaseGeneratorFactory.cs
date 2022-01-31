using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            switch (group.InternalTestType.ToLower())
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKat(group.InternalTestType, group.AlgoMode);

                case "multiblockmessage":
                    return new TestCaseGeneratorMmt(_oracle, group);
            }

            switch (group.TestType.ToLower())
            {
                case "mct":
                    return new TestCaseGeneratorMct(_oracle, group);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
