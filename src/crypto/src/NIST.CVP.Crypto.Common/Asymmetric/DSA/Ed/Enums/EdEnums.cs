using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums
{
    public enum Curve
    {
        [EnumMember(Value = "Edwards25519")]
        Ed25519,
        [EnumMember(Value = "Edwards448")]
        Ed448
    }

    public enum CurveType
    {
        Prime,
        Binary
    }

    public enum SecretGenerationMode
    {
        [EnumMember(Value = "testing candidates")]
        TestingCandidates,

        [EnumMember(Value = "extra bits")]
        ExtraRandomBits
    }
}
