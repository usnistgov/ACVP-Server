using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdSignature : IDsaSignature
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }

        public EdSignature()
        {
            
        }

        public EdSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
