using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.KDF.Enums
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
