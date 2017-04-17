using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class PublicKey
    {
        public BigInteger e { get; set; }
        public BigInteger n { get; set; }
    }
}
