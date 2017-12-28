using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.TLS.Enums
{
    public enum TlsModes
    {
        [Description("v1.0/1.1")]
        v10v11,

        [Description("v1.2")]
        v12
    }
}
