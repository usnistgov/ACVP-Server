using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcSignature : IDsaSignature
    {
        public BigInteger R { get; }
        public BigInteger S { get; }

        public FfcSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
