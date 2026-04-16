using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums
{
    public enum SPDMVersions
    {
        None,

        [EnumMember(Value = "spdm1.1")]
        SPDM11,

        [EnumMember(Value = "spdm1.2")]
        SPDM12,

        [EnumMember(Value = "spdm1.3")]
        SPDM13,
    }
}
