using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPair : IDsaKeyPair
    {
        public BigInteger PublicQ { get; set; }
        public BigInteger PrivateD { get; set; }

        public EdKeyPair()
        {
            
        }

        public EdKeyPair(BigInteger q, BigInteger d)
        {
            PublicQ = q;
            PrivateD = d;
        }

        public EdKeyPair(BigInteger q)
        {
            PublicQ = q;
        }
    }
}