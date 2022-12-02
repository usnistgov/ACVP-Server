
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class RsaDecryptionPrimitiveParameters
    {
        public bool TestPassed { get; set; }
        public int Modulo { get; set; }
        public PrivateKeyModes Mode { get; set; }
    }
}
