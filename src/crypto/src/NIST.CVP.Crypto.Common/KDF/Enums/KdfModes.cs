using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KDF.Enums
{
    public enum KdfModes
    {
        [Description("counter")]
        Counter,

        [Description("feedback")]
        Feedback,

        [Description("double pipeline iteration")]
        Pipeline
    }
}
