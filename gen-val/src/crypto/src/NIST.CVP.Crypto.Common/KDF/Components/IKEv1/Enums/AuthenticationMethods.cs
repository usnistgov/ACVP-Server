using System.ComponentModel;
using System.Runtime.Serialization;

namespace NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums
{
    public enum AuthenticationMethods
    {
        [EnumMember(Value = "dsa")]    // Digital signature algorithm
        Dsa,

        [EnumMember(Value = "pke")]    // Public key encryption
        Pke,

        [EnumMember(Value = "psk")]    // Pre-shared key
        Psk
    }
}
