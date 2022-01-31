using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum LmsSignatureDisposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "modify signature")]
        ModifySignature,

        [EnumMember(Value = "modify message")]
        ModifyMessage,

        [EnumMember(Value = "modify key")]
        ModifyKey,
    }
}
