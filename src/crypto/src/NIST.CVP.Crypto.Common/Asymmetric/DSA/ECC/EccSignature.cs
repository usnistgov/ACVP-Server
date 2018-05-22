using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccSignature : IDsaSignature
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }

        public EccSignature()
        {
            
        }

        public EccSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
