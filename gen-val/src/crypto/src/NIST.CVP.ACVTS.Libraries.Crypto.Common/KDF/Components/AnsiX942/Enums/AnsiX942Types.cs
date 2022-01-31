using System.Runtime.Serialization;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums
{
    public enum AnsiX942Types
    {
        None,

        [EnumMember(Value = "DER")]
        Der,

        [EnumMember(Value = "concatenation")]
        Concat
    }
}
