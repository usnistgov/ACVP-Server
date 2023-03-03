using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums
{
    /// <summary>
    /// LMS labels from https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-208.pdf
    /// </summary>
    public enum LmsMode
    {
        Invalid,

        [EnumMember(Value = "LMS_SHA256_M24_H5")]
        LMS_SHA256_M24_H5,
        [EnumMember(Value = "LMS_SHA256_M24_H10")]
        LMS_SHA256_M24_H10,
        [EnumMember(Value = "LMS_SHA256_M24_H15")]
        LMS_SHA256_M24_H15,
        [EnumMember(Value = "LMS_SHA256_M24_H20")]
        LMS_SHA256_M24_H20,
        [EnumMember(Value = "LMS_SHA256_M24_H25")]
        LMS_SHA256_M24_H25,

        [EnumMember(Value = "LMS_SHA256_M32_H5")]
        LMS_SHA256_M32_H5,
        [EnumMember(Value = "LMS_SHA256_M32_H10")]
        LMS_SHA256_M32_H10,
        [EnumMember(Value = "LMS_SHA256_M32_H15")]
        LMS_SHA256_M32_H15,
        [EnumMember(Value = "LMS_SHA256_M32_H20")]
        LMS_SHA256_M32_H20,
        [EnumMember(Value = "LMS_SHA256_M32_H25")]
        LMS_SHA256_M32_H25,

        [EnumMember(Value = "LMS_SHAKE_M24_H5")]
        LMS_SHAKE_M24_H5,
        [EnumMember(Value = "LMS_SHAKE_M24_H10")]
        LMS_SHAKE_M24_H10,
        [EnumMember(Value = "LMS_SHAKE_M24_H15")]
        LMS_SHAKE_M24_H15,
        [EnumMember(Value = "LMS_SHAKE_M24_H20")]
        LMS_SHAKE_M24_H20,
        [EnumMember(Value = "LMS_SHAKE_M24_H25")]
        LMS_SHAKE_M24_H25,

        [EnumMember(Value = "LMS_SHAKE_M32_H5")]
        LMS_SHAKE_M32_H5,
        [EnumMember(Value = "LMS_SHAKE_M32_H10")]
        LMS_SHAKE_M32_H10,
        [EnumMember(Value = "LMS_SHAKE_M32_H15")]
        LMS_SHAKE_M32_H15,
        [EnumMember(Value = "LMS_SHAKE_M32_H20")]
        LMS_SHAKE_M32_H20,
        [EnumMember(Value = "LMS_SHAKE_M32_H25")]
        LMS_SHAKE_M32_H25
    }
}
