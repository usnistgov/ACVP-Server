using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Enums
{
    public enum KdfModes
    {
        [EnumMember(Value = "counter")]
        Counter,

        [EnumMember(Value = "feedback")]
        Feedback,

        [EnumMember(Value = "double pipeline iteration")]
        Pipeline
    }
}
