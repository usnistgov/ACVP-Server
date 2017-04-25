using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class KeyPair
    {
        public PublicKey PubKey { get; set; }
        public PrivateKey PrivKey { get; set; }

        public KeyPair(BigInteger p, BigInteger q, BigInteger e)
        {
            PubKey.N = p * q;
            PubKey.E = e;
            PubKey.Phi_N = (p - 1) * (q - 1);

            PrivKey.P = p;
            PrivKey.Q = q;
            PrivKey.D = NumberTheory.ModularInverse(PubKey.E, PubKey.Phi_N);
        }
    }
}
