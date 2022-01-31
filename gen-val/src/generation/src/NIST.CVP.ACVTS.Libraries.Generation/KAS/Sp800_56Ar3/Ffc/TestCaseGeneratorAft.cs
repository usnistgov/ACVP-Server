using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.Sp800_56Ar3.Ffc
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

            return (FfcKeyPair)keyPair;
        }
    }
}
