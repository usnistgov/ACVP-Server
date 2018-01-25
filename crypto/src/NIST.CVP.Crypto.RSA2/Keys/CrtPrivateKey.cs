using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.Keys
{
    // Chinese Remainder Theorem
    public class CrtPrivateKey : IRsaPrivateKey
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }

        public BigInteger DMP1 { get; set; }
        public BigInteger DMQ1 { get; set; }
        public BigInteger IQMP { get; set; }
    }
}
