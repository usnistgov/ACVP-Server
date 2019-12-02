using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Generation.Core.Enums
{
    public enum Disposition
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "passed")]
        Passed,

        [EnumMember(Value = "missing")]
        Missing,

        [EnumMember(Value = "failed")]
        Failed
    }
}
