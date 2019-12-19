using System.Collections.Generic;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ffc
{
    public class TestCaseGeneratorVal : TestCaseGeneratorValBase<TestGroup, TestCase, FfcKeyPair>
    {
        public TestCaseGeneratorVal(IOracle oracle, List<KasValTestDisposition> validityTestCaseOptions) 
            : base(oracle, validityTestCaseOptions)
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