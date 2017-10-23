using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Generation.Core.Enums
{
    public enum Disposition
    {
        [Description("none")]
        None,

        [Description("passed")]
        Passed,

        [Description("missing")]
        Missing,

        [Description("failed")]
        Failed
    }
}
