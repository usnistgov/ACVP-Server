using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.Ffc
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        private const string aftTest = "AFT";
        private const string valTest = "VAL";

        public TestCaseGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.TestType.Equals(aftTest, StringComparison.OrdinalIgnoreCase)
                && testGroup.IsSample)
            {
                // When running in sample mode, the ACVP server needs produce vectors as if it were both parties.
                // Since VAL tests do this anyway, we can fall back on its test case generator for producing sample AFT tests.
                var validityTestCaseOptions =
                    new TestCaseExpectationProvider<TestGroup, TestCase, FfcKeyPair>(testGroup, false);
                return new TestCaseGeneratorVal(_oracle, validityTestCaseOptions);
            }

            if (testGroup.TestType.Equals(aftTest, StringComparison.OrdinalIgnoreCase))
            {
                return new TestCaseGeneratorAft(_oracle);
            }

            if (testGroup.TestType.Equals(valTest, StringComparison.OrdinalIgnoreCase))
            {
                var validityTestCaseOptions =
                    new TestCaseExpectationProvider<TestGroup, TestCase, FfcKeyPair>(testGroup, true);
                return new TestCaseGeneratorVal(_oracle, validityTestCaseOptions);
            }

            return new TestCaseGeneratorNull<TestGroup, TestCase, FfcKeyPair>();
        }
    }
}
