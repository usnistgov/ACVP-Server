using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums
{
    public enum Curve
    {
        [EnumMember(Value = "P-192")]
        P192,
        [EnumMember(Value = "P-224")]
        P224,
        [EnumMember(Value = "P-256")]
        P256,
        [EnumMember(Value = "P-384")]
        P384,
        [EnumMember(Value = "P-521")]
        P521,       // Should be 521 not 512.
        [EnumMember(Value = "K-163")]
        K163,
        [EnumMember(Value = "K-233")]
        K233,
        [EnumMember(Value = "K-283")]
        K283,
        [EnumMember(Value = "K-409")]
        K409,
        [EnumMember(Value = "K-571")]
        K571,
        [EnumMember(Value = "B-163")]
        B163,
        [EnumMember(Value = "B-233")]
        B233,
        [EnumMember(Value = "B-283")]
        B283,
        [EnumMember(Value = "B-409")]
        B409,
        [EnumMember(Value = "B-571")]
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

    public enum NonceProviderTypes
    {
        Random,
        Deterministic
    }
}
