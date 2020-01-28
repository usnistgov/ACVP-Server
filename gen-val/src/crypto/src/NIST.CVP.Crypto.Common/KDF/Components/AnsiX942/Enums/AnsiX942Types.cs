using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Components.AnsiX942.Enums
{
    public enum AnsiX942Types
    {
        [EnumMember(Value = "DER")]
        Der,

        [EnumMember(Value = "concatenation")]
        Concat
    }
}
