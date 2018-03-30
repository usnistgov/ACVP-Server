using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Enums
{
    public enum PQFailureReasons
    {
        [Description("none")]
        None,

        [Description("modify p")]
        ModifyP,

        [Description("modify q")]
        ModifyQ,

        [Description("modify seed")]
        ModifySeed
    }

    public enum GFailureReasons
    {
        [Description("none")]
        None,

        [Description("modify g")]
        ModifyG
    }
}
