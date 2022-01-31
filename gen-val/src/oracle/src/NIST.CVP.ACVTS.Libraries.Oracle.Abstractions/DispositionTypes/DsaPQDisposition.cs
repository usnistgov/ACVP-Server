using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum DsaPQDisposition
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
}
