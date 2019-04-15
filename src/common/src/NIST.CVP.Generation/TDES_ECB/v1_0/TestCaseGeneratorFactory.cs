using System;
using System.Linq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_ECB.v1_0
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly string[] _katLabels = KatData.GetLabels();

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            if (_katLabels.Contains(group.InternalTestType, StringComparer.OrdinalIgnoreCase))
            {
                return new TestCaseGeneratorKat(group.InternalTestType);
            }
            else if (group.InternalTestType.Equals("multiblockmessage", StringComparison.OrdinalIgnoreCase))
            {
                return new TestCaseGeneratorMmt(_oracle);
            }
            else if (group.TestType.Equals("mct", StringComparison.OrdinalIgnoreCase))
            {
                return new TestCaseGeneratorMct(_oracle);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
