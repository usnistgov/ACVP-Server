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

        public KeyPair(BigInteger p, BigInteger q, BigInteger e)
        {
            PubKey = new PublicKey {E = e, N = p * q};
            PrivKey = new PrivateKey {P = p, Q = q, D = NumberTheory.ModularInverse(PubKey.E, (p - 1) * (q - 1))};
            PrivKey.ComputeCRT();
        }

        public KeyPair(BigInteger p, BigInteger q, BigInteger e, BigInteger dmp1, BigInteger dmq1, BigInteger iqmp)
        {
            PubKey = new PublicKey {E = e, N = p * q};
            PrivKey = new PrivateKey {P = p, Q = q, DMP1 = dmp1, DMQ1 = dmq1, IQMP = iqmp};
        }

        public KeyPair(BitString p, BitString q, BitString n, BitString e, BitString d)
        {
            PubKey = new PublicKey {E = e.ToPositiveBigInteger(), N = n.ToPositiveBigInteger()};
            PrivKey = new PrivateKey {P = p.ToPositiveBigInteger(), Q = q.ToPositiveBigInteger(), D = d.ToPositiveBigInteger()};
            PrivKey.ComputeCRT();
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

        public BigInteger DMP1 { get; set; }
        public BigInteger DMQ1 { get; set; }
        public BigInteger IQMP { get; set; }

        public void ComputeCRT()
        {
            DMP1 = D % (P - 1);
            DMQ1 = D % (Q - 1);
            IQMP = NumberTheory.ModularInverse(Q, P);
        }
    }
}
