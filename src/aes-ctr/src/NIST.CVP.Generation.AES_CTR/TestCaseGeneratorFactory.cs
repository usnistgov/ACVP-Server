using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_CTR
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
            var testType = group.TestType.ToLower();

            switch (testType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(group.KeyLength, testType);
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
