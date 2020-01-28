using System.Collections.Generic;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ecc
{
    public class TestCaseGeneratorVal : TestCaseGeneratorValBase<TestGroup, TestCase, EccKeyPair>
    {
        public TestCaseGeneratorVal(IOracle oracle, List<KasValTestDisposition> validityTestCaseOptions) : base(oracle, validityTestCaseOptions)
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