using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum XmssSignatureDisposition
    {
        [EnumMember(Value = "no modification")]
        None,

        [EnumMember(Value = "modify signature")]
        ModifySignature,

        [EnumMember(Value = "modify message")]
        ModifyMessage,

        [EnumMember(Value = "modify signature index")]
        ModifyIndex
    }
}
