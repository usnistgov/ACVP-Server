using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Enums
{
    public enum KdfModes
    {
        None,
        [EnumMember(Value = "counter")]
        Counter,

        [EnumMember(Value = "feedback")]
        Feedback,

        [EnumMember(Value = "double pipeline iteration")]
        Pipeline
    }
}
