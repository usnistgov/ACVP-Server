using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum DsaSignatureDisposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "modify s")]
        ModifyS,

        [EnumMember(Value = "modify message")]
        ModifyMessage,

        [EnumMember(Value = "modify r")]
        ModifyR,

        [EnumMember(Value = "modify key")]
        ModifyKey
    }
}
