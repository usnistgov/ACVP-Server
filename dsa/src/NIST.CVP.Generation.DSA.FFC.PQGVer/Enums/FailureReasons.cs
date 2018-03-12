using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Enums
{
    public enum PQFailureReasons
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "modify p")]
        ModifyP,

        [EnumMember(Value = "modify q")]
        ModifyQ,

        [EnumMember(Value = "modify seed")]
        ModifySeed
    }

    public enum GFailureReasons
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "modify g")]
        ModifyG
    }
}
