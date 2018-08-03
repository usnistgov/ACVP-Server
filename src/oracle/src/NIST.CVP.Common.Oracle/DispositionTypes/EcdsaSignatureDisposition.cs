using System.Runtime.Serialization;

namespace NIST.CVP.Common.Oracle.DispositionTypes
{
    public enum EcdsaSignatureDisposition
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
        ModifyKey,
    }
}
