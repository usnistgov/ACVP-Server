using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.KeyGen
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
            switch (testGroup.TestType.ToLower())
            {
                case "kat":
                    return new TestCaseGeneratorKat(testGroup, _oracle);

                case "aft":
                    return new TestCaseGeneratorAft(_oracle);

                case "gdt":
                    return new TestCaseGeneratorGdt(_oracle);

                default:
                    throw new Exception("Invalid test type");
            }
        }
    }
}
