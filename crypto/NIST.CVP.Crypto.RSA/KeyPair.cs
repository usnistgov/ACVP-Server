using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    public class KeyPair
    {
        public PublicKey PubKey { get; set; }
        public PrivateKey PrivKey { get; set; }

        public KeyPair() { }

        public KeyPair(BigInteger p, BigInteger q, BigInteger e)
        {
            PubKey = new PublicKey {E = e, N = p * q};
            PrivKey = new PrivateKey {P = p, Q = q, D = NumberTheory.ModularInverse(PubKey.E, (p - 1) * (q - 1))};
        }

        public KeyPair(BitString p, BitString q, BitString n, BitString e, BitString d)
        {
            PubKey = new PublicKey {E = e.ToPositiveBigInteger(), N = n.ToPositiveBigInteger()};
            PrivKey = new PrivateKey {P = p.ToPositiveBigInteger(), Q = q.ToPositiveBigInteger(), D = d.ToPositiveBigInteger()};
        }
    }

    public class PublicKey
    {
        public BigInteger E { get; set; }
        public BigInteger N { get; set; }
    }

    public class PrivateKey
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger D { get; set; }
        public BigInteger Phi_N { get { return (P - 1) * (Q - 1); } }
    }
}
