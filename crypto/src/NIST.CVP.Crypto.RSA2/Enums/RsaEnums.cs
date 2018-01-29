using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Enums
{
    public enum PrimeGenModes
    {
        [Description("B.3.2")]
        B32,

        [Description("B.3.3")]
        B33,

        [Description("B.3.4")]
        B34,

        [Description("B.3.5")]
        B35,

        [Description("B.3.6")]
        B36
    }

    public enum PrimeTestModes
    {
        [Description("C.2")]
        C2,

        [Description("C.3")]
        C3
    }

    public enum SignatureSchemes
    {
        [Description("ansx9.31")]
        Ansx931,

        [Description("pkcs1v15")]
        Pkcs1v15,

        [Description("pss")]
        Pss
    }
}
