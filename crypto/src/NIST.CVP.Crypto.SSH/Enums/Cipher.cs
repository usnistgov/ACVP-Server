using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NIST.CVP.Crypto.SSH.Enums
{
    public enum Cipher
    {
        [Description("tdes")]
        TDES,

        [Description("aes-128")]
        AES128,

        [Description("aes-192")]
        AES192,

        [Description("aes-256")]
        AES256
    }
}
