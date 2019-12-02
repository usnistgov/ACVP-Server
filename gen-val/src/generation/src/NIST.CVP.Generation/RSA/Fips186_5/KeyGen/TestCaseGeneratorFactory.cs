using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen
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
                case "gdt":
                    return new TestCaseGeneratorAft(_oracle);

                default:
                    throw new Exception("Invalid test type");
            }
        }
    }
}