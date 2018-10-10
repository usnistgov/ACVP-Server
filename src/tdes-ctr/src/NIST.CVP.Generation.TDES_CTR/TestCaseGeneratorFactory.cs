using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_CTR
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
            var internalTestType = group.InternalTestType.ToLower();
            var testType = group.TestType.ToLower();

            switch (internalTestType)
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKat(internalTestType);

                case "singleblock":
                    return new TestCaseGeneratorSingleBlock(_oracle);

                case "partialblock":
                    return new TestCaseGeneratorPartialBlock(_oracle);
            }

            switch (testType)
            {
                case "ctr":
                    return new TestCaseGeneratorCounter(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
