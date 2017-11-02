using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.Enums
{
    public enum SigFailureReasons
    {
        [Description("none")]
        None,

        [Description("modify s")]
        ModifyS,

        [Description("modify message")]
        ModifyMessage,

        [Description("modify r")]
        ModifyR,

        [Description("modify key")]
        ModifyKey,
    }
}
