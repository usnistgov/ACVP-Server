using System.ComponentModel;

namespace NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums
{
    public enum AuthenticationMethods
    {
        [Description("dsa")]    // Digital signature algorithm
        Dsa,

        [Description("pke")]    // Public key encryption
        Pke,

        [Description("psk")]    // Pre-shared key
        Psk
    }
}
