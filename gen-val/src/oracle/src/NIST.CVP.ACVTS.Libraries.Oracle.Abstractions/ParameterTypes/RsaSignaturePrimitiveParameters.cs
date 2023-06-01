using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes
{
    public class RsaSignaturePrimitiveParameters
    {
        public int Modulo { get; set; }
        public PrivateKeyModes KeyFormat { get; set; }
        public BitString PublicExponent { get; set; }
        public string Disposition { get; set; } = "none";
    }
}
