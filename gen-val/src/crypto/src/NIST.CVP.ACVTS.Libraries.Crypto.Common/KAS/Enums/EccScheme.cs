using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums
{
    public enum EccScheme
    {
        None,
        [EnumMember(Value = "fullUnified")]
        FullUnified,
        [EnumMember(Value = "fullMqv")]
        FullMqv,
        [EnumMember(Value = "ephemeralUnified")]
        EphemeralUnified,
        [EnumMember(Value = "onePassUnified")]
        OnePassUnified,
        [EnumMember(Value = "onePassMqv")]
        OnePassMqv,
        [EnumMember(Value = "onePassDh")]
        OnePassDh,
        [EnumMember(Value = "staticUnified")]
        StaticUnified,
        [EnumMember(Value = "componentTest")]
        ComponentTest
    }
}
