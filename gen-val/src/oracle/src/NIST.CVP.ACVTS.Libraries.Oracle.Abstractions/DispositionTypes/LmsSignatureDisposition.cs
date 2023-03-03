using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum LmsSignatureDisposition
    {
        [EnumMember(Value = "no modification")]
        None,

        [EnumMember(Value = "modify signature")]
        ModifySignature,

        [EnumMember(Value = "modify message")]
        ModifyMessage,

        [EnumMember(Value = "modify key")]
        ModifyKey,
        
        [EnumMember(Value = "modify signature header")]
        ModifyHeader
    }
}
