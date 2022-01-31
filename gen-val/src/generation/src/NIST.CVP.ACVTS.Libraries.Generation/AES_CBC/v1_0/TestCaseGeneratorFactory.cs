using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CBC.v1_0
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
            var internalTestType = testGroup.InternalTestType.ToLower();

            switch (internalTestType)
            {
                case "gfsbox":
                case "keysbox":
                case "vartxt":
                case "varkey":
                    return new TestCaseGeneratorKnownAnswer(testGroup.KeyLength, internalTestType);

                case "hact":
                    return new TestCaseGeneratorHact(_oracle);
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
