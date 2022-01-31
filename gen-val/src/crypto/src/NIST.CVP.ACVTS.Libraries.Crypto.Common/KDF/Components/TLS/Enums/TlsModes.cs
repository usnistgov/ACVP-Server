using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums
{
    public enum TlsModes
    {
        [EnumMember(Value = "v1.0/1.1")]
        v10v11,

        [EnumMember(Value = "v1.2")]
        v12,

        [EnumMember(Value = "v1.2_ems")]
        v12_extendedMasterSecret
    }
}
