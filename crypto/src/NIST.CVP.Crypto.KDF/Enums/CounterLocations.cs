using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.KDF.Enums
{
    public enum CounterLocations
    {
        [Description("none")]
        None,

        [Description("before")]
        BeforeFixedData,

        [Description("middle")]
        MiddleFixedData,

        [Description("after")]
        AfterFixedData,
    }
}
