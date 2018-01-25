using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    public interface IRsaPrivateKey
    {
        BigInteger P { get; set; }
        BigInteger Q { get; set; }
    }
}
