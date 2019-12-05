using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Enums
{
    public enum IfcScheme
    {
        [EnumMember(Value = "KAS1-basic")]
        Kas1_basic,
        [EnumMember(Value = "KAS1-Party_V-confirmation")]
        Kas1_partyV_keyConfirmation,
        [EnumMember(Value = "KAS2-basic")]
        Kas2_basic,
        [EnumMember(Value = "KAS2-bilateral-confirmation")]
        Kas2_bilateral_keyConfirmation,
        [EnumMember(Value = "KAS2-Party_V-confirmation")]
        Kas2_partyV_keyConfirmation,
        [EnumMember(Value = "KAS2-Party_U-confirmation")]
        Kas2_partyU_keyConfirmation,
        [EnumMember(Value = "KTS-OAEP-basic")]
        Kts_oaep_basic,
        [EnumMember(Value = "KTS-OAEP-Party_V-confirmation")]
        Kts_oaep_partyV_keyConfirmation
    }
}