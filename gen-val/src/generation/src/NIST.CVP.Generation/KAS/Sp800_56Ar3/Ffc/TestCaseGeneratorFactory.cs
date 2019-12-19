using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Helpers;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ffc
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
                var validityTestCaseOptions = TestCaseDispositionHelper.PopulateValidityTestCaseOptions(testGroup, false);
                return new TestCaseGeneratorVal(_oracle, validityTestCaseOptions);
            }
            
            if (testGroup.TestType.Equals(aftTest, StringComparison.OrdinalIgnoreCase))
            {
                return new TestCaseGeneratorAft(_oracle);
            }

            if (testGroup.TestType.Equals(valTest, StringComparison.OrdinalIgnoreCase))
            {
                var validityTestCaseOptions = TestCaseDispositionHelper.PopulateValidityTestCaseOptions(testGroup, true);
                return new TestCaseGeneratorVal(_oracle, validityTestCaseOptions);
            }

            return new TestCaseGeneratorNull<TestGroup, TestCase, FfcKeyPair>();
        }
    }
}