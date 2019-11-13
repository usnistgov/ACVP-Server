using System.Runtime.Serialization;

namespace NIST.CVP.Common.Oracle.DispositionTypes
{
    public enum KasIfcValTestDisposition
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "Success - leading zero nibble shared secret Z.")]
        SuccessLeadingZeroNibbleZ,
        [EnumMember(Value = "Failure - IUT should calculate different tag due to Z value changed.")]
        FailChangedZ,
        [EnumMember(Value = "Failure - IUT should calculate different value due to DKM value changed.")]
        FailChangedDkm,
        [EnumMember(Value = "Failure - IUT should calculate different value due to MacData value changed.")]
        FailChangedMacData,
        [EnumMember(Value = "Failure - IUT should calculate different value due to Tag value changed.")]
        FailChangedTag,
        [EnumMember(Value = "Failure - KeyConfirmation bits should have been removed from the final derived key.")]
        FailKeyConfirmationBits
    }
}