using System.Collections.Generic;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc
{
    public class TestCaseGeneratorVal : TestCaseGeneratorValBase<TestGroup, TestCase, EccKeyPair>
    {
        public TestCaseGeneratorVal(IOracle oracle, ITestCaseExpectationProvider<KasSscTestCaseExpectation> testCaseExpectationProvider, int numberOfTestCasesToGenerate) 
            : base(oracle, testCaseExpectationProvider, numberOfTestCasesToGenerate)
        {
        }

        protected override EccKeyPair GetKey(IDsaKeyPair keyPair)
        {
            if (keyPair == null)
                return new EccKeyPair();

            return (EccKeyPair) keyPair;
        }
    }
}