using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TDES_ECB
{
    public enum FunctionValues
    {
        Encryption,
        Decryption
    }

    public enum KeyOptionValues
    {
        OneKey = 1,
        TwoKey = 2,
        ThreeKey = 3
    }
}
