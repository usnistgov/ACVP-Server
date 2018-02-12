using System.Numerics;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class PpcResult
    {
        public BigInteger Prime { get; }
        public BigInteger Prime1 { get; }
        public BigInteger Prime2 { get; }
        public BigInteger PrimeSeed { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public PpcResult(string fail)
        {
            ErrorMessage = fail;
        }

        public PpcResult(BigInteger p, BigInteger p1, BigInteger p2, BigInteger pSeed)
        {
            Prime = p;
            Prime1 = p1;
            Prime2 = p2;
            PrimeSeed = pSeed;
        }
    }
}
