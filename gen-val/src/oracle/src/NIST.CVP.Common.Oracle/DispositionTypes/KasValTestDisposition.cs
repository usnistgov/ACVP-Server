using System.Runtime.Serialization;

namespace NIST.CVP.Common.Oracle.DispositionTypes
{
    public enum KasValTestDisposition
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "Success - leading zero nibble shared secret Z.")]
        SuccessLeadingZeroNibbleZ,
        [EnumMember(Value = "Success - leading zero nibble DKM.")]
        SuccessLeadingZeroNibbleDkm,
        [EnumMember(Value = "Failure - IUT should detect issue in IUT static private key.")]
        FailAssuranceIutStaticPrivateKey,
        [EnumMember(Value = "Failure - IUT should detect issue in IUT static public key.")]
        FailAssuranceIutStaticPublicKey,
        [EnumMember(Value = "Failure - IUT should detect issue in ACVP server static public key.")]
        FailAssuranceServerStaticPublicKey,
        [EnumMember(Value = "Failure - IUT should detect issue in ACVP server ephemeral public key.")]
        FailAssuranceServerEphemeralPublicKey,
        [EnumMember(Value = "Failure - IUT should calculate different tag due to Z value changed.")]
        FailChangedZ,
        [EnumMember(Value = "Failure - IUT should calculate different tag due to DKM value changed.")]
        FailChangedDkm,
        [EnumMember(Value = "Failure - IUT should calculate different tag due to OI value changed.")]
        FailChangedOi,
        [EnumMember(Value = "Failure - IUT should calculate different tag due to MacData value changed.")]
        FailChangedMacData,
        [EnumMember(Value = "Failure - IUT should calculate different tag due to Tag value changed.")]
        FailChangedTag,
        [EnumMember(Value = "Failure - KeyConfirmation bits should have been removed from the final derived key.")]
        FailKeyConfirmationBits
    }
}
