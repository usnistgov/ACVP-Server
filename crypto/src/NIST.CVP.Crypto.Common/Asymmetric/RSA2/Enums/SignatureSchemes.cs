using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums
{
    public enum SignatureSchemes
    {
        [EnumMember(Value = "ansx9.31")]
        Ansx931,

        [EnumMember(Value = "pkcs1v1.5")]
        Pkcs1v15,

        [EnumMember(Value = "pss")]
        Pss
    }
}
