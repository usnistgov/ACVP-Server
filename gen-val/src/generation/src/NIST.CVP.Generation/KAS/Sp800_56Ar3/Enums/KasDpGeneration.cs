using System.Runtime.Serialization;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums
{
    /// <summary>
    /// Enum represents the different methods of getting domain parameters.
    /// </summary>
    public enum KasDpGeneration
    {
        None,
        #region FFC
        [EnumMember(Value = "MODP-2048")]
        Modp2048,
        [EnumMember(Value = "MODP-3072")]
        Modp3072,
        [EnumMember(Value = "MODP-4096")]
        Modp4096,
        [EnumMember(Value = "MODP-6144")]
        Modp6144,
        [EnumMember(Value = "MODP-8192")]
        Modp8192,
        [EnumMember(Value = "ffdhe2048")]
        Ffdhe2048,
        [EnumMember(Value = "ffdhe3072")]
        Ffdhe3072,
        [EnumMember(Value = "ffdhe4096")]
        Ffdhe4096,
        [EnumMember(Value = "ffdhe6144")]
        Ffdhe6144,
        [EnumMember(Value = "ffdhe8192")]
        Ffdhe8192,
        [EnumMember(Value = "FB")]
        Fb,
        [EnumMember(Value = "FC")]
        Fc,
        #endregion FFC
        #region ECC
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
        B571,
        #endregion ECC
    }
}