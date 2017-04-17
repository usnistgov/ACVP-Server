using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class PrivateKey
    {
        public BigInteger p { get; set; }
        public BigInteger q { get; set; }
        public BigInteger d { get; set; }
    }
}
