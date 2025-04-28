using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums
{
    public enum Curve
    {
        [EnumMember(Value = "Curve25519")]
        Curve25519,
        [EnumMember(Value = "Curve448")]
        Curve448
    }
}
