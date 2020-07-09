using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ffc
{
    public class TestCaseGeneratorVal : TestCaseGeneratorValBase<TestGroup, TestCase, FfcKeyPair>
    {
        public TestCaseGeneratorVal(IOracle oracle, ITestCaseExpectationProvider<KasSscTestCaseExpectation> testCaseExpectationProvider, int numberOfTestCasesToGenerate) 
            : base(oracle, testCaseExpectationProvider, numberOfTestCasesToGenerate)
        {
        }

        protected override FfcKeyPair GetKey(IDsaKeyPair keyPair)
        {
            if (keyPair == null)
                return new FfcKeyPair();

            return (FfcKeyPair) keyPair;
        }
    }
}