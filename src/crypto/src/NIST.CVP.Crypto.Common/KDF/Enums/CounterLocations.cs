using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KDF.Enums
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
