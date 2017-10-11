using System;
using System.ComponentModel;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.Enums
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
        ModifyKey
    }
}
