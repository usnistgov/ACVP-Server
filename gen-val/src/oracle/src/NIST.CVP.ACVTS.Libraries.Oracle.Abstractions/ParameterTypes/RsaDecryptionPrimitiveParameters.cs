using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class RsaDecryptionPrimitiveParameters
    {
        public bool TestPassed { get; set; }
        public int Modulo { get; set; }
        public PrivateKeyModes Mode { get; set; }
        public string Disposition { get; set; } = "none";
        public BitString PublicExponent { get; set; }
        public PublicExponentModes PublicExponentMode { get; set; }
    }
}
