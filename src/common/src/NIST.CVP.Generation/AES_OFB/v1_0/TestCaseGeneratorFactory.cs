using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_OFB.v1_0
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
            var testType = testGroup.InternalTestType.ToLower();

            switch (testType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(testGroup.KeyLength, testType);
                case "mct":
                    return new TestCaseGeneratorMct(_oracle);
                case "mmt":
                    return new TestCaseGeneratorMmt(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
