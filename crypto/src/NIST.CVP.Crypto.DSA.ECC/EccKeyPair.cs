using System.Numerics;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccKeyPair : IDsaKeyPair
    {
        public EccPoint PublicQ { get; }
        public BigInteger PrivateD { get; }

        public EccKeyPair(EccPoint q, BigInteger d)
        {
            PublicQ = q;
            PrivateD = d;
        }

        public EccKeyPair(EccPoint q)
        {
            PublicQ = q;
        }

        public EccKeyPair(BigInteger d)
        {
            PrivateD = d;
        }
    }
}