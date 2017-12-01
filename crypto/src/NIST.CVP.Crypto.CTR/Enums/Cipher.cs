using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.CTR.Enums
{
    public enum Cipher
    {
        [Description("AES")]
        AES,

        [Description("TDES")]
        TDES
    }
}
