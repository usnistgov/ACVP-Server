using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_CBC
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
            var testType = testGroup.TestType.ToLower();
            var katType = testGroup.InternalTestType.ToLower();

            switch (katType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(testGroup.KeyLength, katType);
            }
            
            switch (testType)
            {
                case "mct":
                    return new TestCaseGeneratorMct(_oracle);
                case "aft":
                    return new TestCaseGeneratorMmt(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
