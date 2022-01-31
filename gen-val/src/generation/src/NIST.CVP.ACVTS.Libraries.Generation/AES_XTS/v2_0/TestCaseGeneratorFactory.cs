using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_XTS.v2_0
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
            return testGroup.InternalTestType switch
            {
                "SingleDataUnit" => new TestCaseGeneratorSingleDataUnit(_oracle),
                "MultipleDataUnit" => new TestCaseGeneratorMultipleDataUnit(_oracle),
                _ => throw new ArgumentException()
            };
        }
    }
}
