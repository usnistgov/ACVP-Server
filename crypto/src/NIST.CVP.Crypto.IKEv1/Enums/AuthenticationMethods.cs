using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.IKEv1.Enums
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
