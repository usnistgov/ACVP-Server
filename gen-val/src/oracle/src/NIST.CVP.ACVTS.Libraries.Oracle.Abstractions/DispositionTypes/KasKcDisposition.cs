using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum KasKcDisposition
    {
        [EnumMember(Value = "Success")]
        Success,
        [EnumMember(Value = "LeadingZeroNibbleKey")]
        LeadingZeroNibbleKey,
        [EnumMember(Value = "LeadingZeroByteKey")]
        LeadingZeroByteKey,
        [EnumMember(Value = "LeadingOneBitKey")]
        LeadingOneBitKey
    }
}
