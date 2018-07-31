using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPair : IDsaKeyPair
    {
        public EdPoint PublicQ { get; set; } = new EdPoint();
        public BigInteger PrivateD { get; set; }

        public EdKeyPair()
        {
            
        }

        public EdKeyPair(EdPoint q, BigInteger d)
        {
            PublicQ = q;
            PrivateD = d;
        }

        public EdKeyPair(EdPoint q)
        {
            PublicQ = q;
        }

        public EdKeyPair(BigInteger d)
        {
            PrivateD = d;
        }
    }
}