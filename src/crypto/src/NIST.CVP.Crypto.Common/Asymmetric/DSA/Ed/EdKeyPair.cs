using NIST.CVP.Math;
using System;
using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPair : IDsaKeyPair
    {
        public BitString PublicQ { get; set; }
        public BigInteger PrivateD { get; set; }

        public EdKeyPair()
        {
            
        }

        public EdKeyPair(BitString q, BigInteger d)
        {
            PublicQ = q;
            PrivateD = d;
        }

        public EdKeyPair(BitString q)
        {
            PublicQ = q;
        }
    }
}