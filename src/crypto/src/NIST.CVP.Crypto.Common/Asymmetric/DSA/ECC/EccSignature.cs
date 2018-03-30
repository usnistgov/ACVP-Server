using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccSignature : IDsaSignature
    {
        public BigInteger R { get; }
        public BigInteger S { get; }

        public EccSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
