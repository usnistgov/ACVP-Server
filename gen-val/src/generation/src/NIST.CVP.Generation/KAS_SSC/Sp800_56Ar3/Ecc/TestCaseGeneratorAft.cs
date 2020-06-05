using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc
{
    public class TestCaseGeneratorAft : TestCaseGeneratorAftBase<TestGroup, TestCase, EccKeyPair>
    {
        public TestCaseGeneratorAft(IOracle oracle) : base(oracle)
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