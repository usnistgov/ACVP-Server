using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Ffc
{
    public class TestCase : TestCaseBase<TestGroup, TestCase, FfcKeyPair>
    {
        public override FfcKeyPair StaticKeyServer { get; set; } = new FfcKeyPair();
        public override FfcKeyPair EphemeralKeyServer { get; set; } = new FfcKeyPair();
        public override FfcKeyPair StaticKeyIut { get; set; } = new FfcKeyPair();
        public override FfcKeyPair EphemeralKeyIut { get; set; } = new FfcKeyPair();
    }
}