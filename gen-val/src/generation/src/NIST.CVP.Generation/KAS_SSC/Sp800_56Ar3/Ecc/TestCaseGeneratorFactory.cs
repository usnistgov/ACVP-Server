using System;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Helpers;
using NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.TestCaseExpectations;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc
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
                var testCaseExpectationProvider = new TestCaseExpectationProvider(testGroup.IsSample, false);
                return new TestCaseGeneratorVal(_oracle, testCaseExpectationProvider, testCaseExpectationProvider.ExpectationCount);
            }
            
            if (testGroup.TestType.Equals(aftTest, StringComparison.OrdinalIgnoreCase))
            {
                return new TestCaseGeneratorAft(_oracle);
            }

            if (testGroup.TestType.Equals(valTest, StringComparison.OrdinalIgnoreCase))
            {
                var testCaseExpectationProvider = new TestCaseExpectationProvider(testGroup.IsSample, true);
                return new TestCaseGeneratorVal(_oracle, testCaseExpectationProvider, testCaseExpectationProvider.ExpectationCount);
            }

            return new TestCaseGeneratorNull<TestGroup, TestCase, EccKeyPair>();
        }
    }
}