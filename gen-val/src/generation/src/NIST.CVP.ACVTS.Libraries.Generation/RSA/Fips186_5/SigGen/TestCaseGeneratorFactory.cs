using System;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
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
            var internalTestType = testGroup.InternalTestType.ToLower();

            if (internalTestType == "hact")
            {
                return new TestCaseGeneratorHact(_oracle);
            }
            else
            {
                return new TestCaseGenerator(_oracle);
            }
        }
    }
}
