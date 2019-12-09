using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums
{
    public enum KasScheme
    {
        None,
        [EnumMember(Value = "dhHybrid1")]
        FfcDhHybrid1,
        [EnumMember(Value = "mqv2")]
        FfcMqv2,
        [EnumMember(Value = "dhEphem")]
        FfcDhEphem,
        [EnumMember(Value = "dhHybridOneFlow")]
        FfcDhHybridOneFlow,
        [EnumMember(Value = "mqv1")]
        FfcMqv1,
        [EnumMember(Value = "dhOneFlow")]
        FfcDhOneFlow,
        [EnumMember(Value = "dhStatic")]
        FfcDhStatic,
        [EnumMember(Value = "fullUnified")]
        EccFullUnified,
        [EnumMember(Value = "fullMqv")]
        EccFullMqv,
        [EnumMember(Value = "ephemeralUnified")]
        EccEphemeralUnified,
        [EnumMember(Value = "onePassUnified")]
        EccOnePassUnified,
        [EnumMember(Value = "onePassMqv")]
        EccOnePassMqv,
        [EnumMember(Value = "onePassDh")]
        EccOnePassDh,
        [EnumMember(Value = "staticUnified")]
        EccStaticUnified,
    }
}