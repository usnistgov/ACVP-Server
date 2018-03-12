using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums
{
    public enum Curve
    {
        [EnumMember(Value = "p-192")]
        P192,
        [EnumMember(Value = "p-224")]
        P224,
        [EnumMember(Value = "p-256")]
        P256,
        [EnumMember(Value = "p-384")]
        P384,
        [EnumMember(Value = "p-521")]
        P521,       // Should be 521 not 512.
        [EnumMember(Value = "k-163")]
        K163,
        [EnumMember(Value = "k-233")]
        K233,
        [EnumMember(Value = "k-283")]
        K283,
        [EnumMember(Value = "k-409")]
        K409,
        [EnumMember(Value = "k-571")]
        K571,
        [EnumMember(Value = "b-163")]
        B163,
        [EnumMember(Value = "b-233")]
        B233,
        [EnumMember(Value = "b-283")]
        B283,
        [EnumMember(Value = "b-409")]
        B409,
        [EnumMember(Value = "b-571")]
        B571
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
