using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0
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
            var direction = testGroup.Function.ToLower();

            if (testGroup.CmacType == CmacTypes.TDES)
            {
                if (direction == "gen")
                {
                    return new TestCaseGeneratorGenTdes(_oracle);
                }

                if (direction == "ver")
                {
                    return new TestCaseGeneratorVerTdes(_oracle);
                }
            }
            else
            {
                if (direction == "gen")
                {
                    return new TestCaseGeneratorGenAes(_oracle);
                }

                if (direction == "ver")
                {
                    return new TestCaseGeneratorVerAes(_oracle);
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}
