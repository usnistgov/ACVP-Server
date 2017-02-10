using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES
{
    public enum DirectionValues
    {
        Encrypt,
        Decrypt
    }

    public enum ModeValues
    {
        ECB,
        CBC,
        OFB,
        CFB1,
        CFB8
    }
}
