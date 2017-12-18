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

        [Description("before fixed data")]
        BeforeFixedData,

        [Description("middle fixed data")]
        MiddleFixedData,

        [Description("after fixed data")]
        AfterFixedData,

        [Description("before iterator")]
        BeforeIterator,
    }
}
