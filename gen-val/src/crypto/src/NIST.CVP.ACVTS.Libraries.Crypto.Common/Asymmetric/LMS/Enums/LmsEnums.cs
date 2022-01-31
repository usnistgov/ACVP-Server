using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums
{
    public enum LmsType
    {
        [EnumMember(Value = "LMS_SHA256_M32_H5")]
        LMS_SHA256_M32_H5,
        [EnumMember(Value = "LMS_SHA256_M32_H10")]
        LMS_SHA256_M32_H10,
        [EnumMember(Value = "LMS_SHA256_M32_H15")]
        LMS_SHA256_M32_H15,
        [EnumMember(Value = "LMS_SHA256_M32_H20")]
        LMS_SHA256_M32_H20,
        [EnumMember(Value = "LMS_SHA256_M32_H25")]
        LMS_SHA256_M32_H25
    }

    public enum LmotsType
    {
        [EnumMember(Value = "LMOTS_SHA256_N32_W1")]
        LMOTS_SHA256_N32_W1,
        [EnumMember(Value = "LMOTS_SHA256_N32_W2")]
        LMOTS_SHA256_N32_W2,
        [EnumMember(Value = "LMOTS_SHA256_N32_W4")]
        LMOTS_SHA256_N32_W4,
        [EnumMember(Value = "LMOTS_SHA256_N32_W8")]
        LMOTS_SHA256_N32_W8
    }
}
