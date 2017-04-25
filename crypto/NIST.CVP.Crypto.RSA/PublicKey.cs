using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class PublicKey
    {
        public BigInteger E { get; set; }
        public BigInteger N { get; set; }
        public BigInteger Phi_N { get; set; }
    }
}
