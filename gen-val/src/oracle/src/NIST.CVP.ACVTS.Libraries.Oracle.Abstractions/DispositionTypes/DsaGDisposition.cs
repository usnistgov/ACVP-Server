using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes
{
    public enum DsaGDisposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "modify g")]
        ModifyG
    }
}
