using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcSignature : IDsaSignature
    {
        public BigInteger R { get; set; }
        public BigInteger S { get; set; }

        public FfcSignature()
        {

        }

        public FfcSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
