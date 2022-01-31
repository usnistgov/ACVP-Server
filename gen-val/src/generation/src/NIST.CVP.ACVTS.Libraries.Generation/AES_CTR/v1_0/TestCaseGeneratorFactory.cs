using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0
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
            var testType = group.InternalTestType.ToLower();

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
                case "ctr":
                    return new TestCaseGeneratorCounter(_oracle);
                case "rfc3686":
                    return new TestCaseGeneratorRfc(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
