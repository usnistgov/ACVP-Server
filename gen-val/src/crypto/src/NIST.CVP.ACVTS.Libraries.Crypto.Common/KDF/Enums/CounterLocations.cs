using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums
{
    public enum CounterLocations
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "before fixed data")]
        BeforeFixedData,

        [EnumMember(Value = "middle fixed data")]
        MiddleFixedData,

        [EnumMember(Value = "after fixed data")]
        AfterFixedData,

        [EnumMember(Value = "before iterator")]
        BeforeIterator,
    }
}
