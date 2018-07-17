using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        
        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            var testType = group.TestType.ToLower();

            switch (testType)
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKnownAnswer(group);

                case "singleblock":
                    return new TestCaseGeneratorSingleBlock(_oracle);
                    
                case "partialblock":
                    return new TestCaseGeneratorPartialBlock(_oracle);
                    
                case "counter":
                    return new TestCaseGeneratorCounter(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
