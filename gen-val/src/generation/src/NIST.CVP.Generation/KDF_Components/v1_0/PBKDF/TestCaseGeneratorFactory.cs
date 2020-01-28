using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
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
            var testCaseGenerator = new TestCaseGenerator(_oracle);
            
            return testCaseGenerator;
        }
    }
}