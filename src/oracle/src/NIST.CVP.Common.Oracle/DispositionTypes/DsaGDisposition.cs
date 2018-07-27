using System.Runtime.Serialization;

namespace NIST.CVP.Common.Oracle.DispositionTypes
{
    public enum DsaGDisposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "modify g")]
        ModifyG
    }
}
