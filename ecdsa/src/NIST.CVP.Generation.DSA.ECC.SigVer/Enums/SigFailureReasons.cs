using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.Enums
{
    public enum SigFailureReasons
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
