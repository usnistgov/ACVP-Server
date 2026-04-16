using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum XecdhKeyDisposition
    {
        [EnumMember(Value = "none")]
        None,

        // Public keys with the most significant bit set are still valid.
        // The IUT is required to mask off this bit when performing XECDH.
        [EnumMember(Value = "most significant bit set")]
        MsbSet,

        [EnumMember(Value = "too short")]
        TooShort,

        [EnumMember(Value = "too long")]
        TooLong,
    }
}
