using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Common.Oracle.ParameterTypes
{
    public class RsaSignaturePrimitiveParameters
    {
        public int Modulo { get; set; }
        public PrivateKeyModes KeyFormat { get; set; }
    }
}
