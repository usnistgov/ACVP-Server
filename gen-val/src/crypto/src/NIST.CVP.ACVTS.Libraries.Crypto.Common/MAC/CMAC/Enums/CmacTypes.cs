using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums
{
    public enum CmacTypes
    {
        [EnumMember(Value = "CMAC-AES128")]
        AES128,
        [EnumMember(Value = "CMAC-AES192")]
        AES192,
        [EnumMember(Value = "CMAC-AES256")]
        AES256,
        [EnumMember(Value = "CMAC-TDES")]
        TDES
    }
}
