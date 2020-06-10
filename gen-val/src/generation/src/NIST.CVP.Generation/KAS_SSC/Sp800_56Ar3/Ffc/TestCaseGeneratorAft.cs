using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ffc
{
    public class TestCaseGeneratorAft : TestCaseGeneratorAftBase<TestGroup, TestCase, FfcKeyPair>
    {
        public TestCaseGeneratorAft(IOracle oracle) : base(oracle)
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