using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class RsaDpSp80056Br2Parameters
    {
        public bool TestPassed { get; set; }
        public int Modulo { get; set; }
        public PrivateKeyModes KeyFormat { get; set; }
    }
}
