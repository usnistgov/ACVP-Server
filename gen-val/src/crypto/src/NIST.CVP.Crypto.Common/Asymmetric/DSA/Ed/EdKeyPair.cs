using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPair : IDsaKeyPair
    {
        public BitString PublicQ { get; set; }
        public BitString PrivateD { get; set; }

        public EdKeyPair()
        {
            
        }

        public EdKeyPair(BitString q, BitString d)
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