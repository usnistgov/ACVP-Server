using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup @group)
        {

            switch (group.InternalTestType.ToLower())
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKat(group.InternalTestType);

                case "multiblockmessage":
                    return new TestCaseGeneratorMmt(_oracle);
                    
                case "mct":
                    return new TestCaseGeneratorMct(_oracle);

            }

            return new TestCaseGeneratorNull();
        }
    }
}
