using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202
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
            return testGroup.TestType.ToLower() switch
            {
                "aft" => new TestCaseGeneratorAft(_oracle),
                _ => throw new ArgumentException($"Test type not found: {testGroup.TestType}")
            };
        }
    }
}
