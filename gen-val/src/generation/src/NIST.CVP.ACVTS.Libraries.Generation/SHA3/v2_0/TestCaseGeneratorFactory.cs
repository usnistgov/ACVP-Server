using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0
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
                "mct" => new TestCaseGeneratorMct(_oracle),
                "ldt" => new TestCaseGeneratorLdt(_oracle),
                _ => throw new ArgumentException($"Test type not found: {testGroup.TestType}")
            };
        }
    }
}
