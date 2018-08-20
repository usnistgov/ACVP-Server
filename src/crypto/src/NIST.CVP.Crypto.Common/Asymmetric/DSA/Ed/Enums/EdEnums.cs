using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums
{
    public enum Curve
    {
        [EnumMember(Value = "ed-25519")]
        Ed25519,
        [EnumMember(Value = "ed-448")]
        Ed448
    }

    public enum SecretGenerationMode
    {
        [EnumMember(Value = "testing candidates")]
        TestingCandidates,

        [EnumMember(Value = "extra bits")]
        ExtraRandomBits
    }
}
