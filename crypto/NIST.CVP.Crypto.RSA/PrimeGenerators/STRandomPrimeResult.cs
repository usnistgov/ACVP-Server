using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class STRandomPrimeResult
    {
        public BigInteger Prime { get; private set; }
        public BigInteger PrimeSeed { get; private set; }
        public BigInteger PrimeGenCounter { get; private set; }
        public string ErrorMessage { get; private set; }

        public STRandomPrimeResult(BigInteger prime, BigInteger primeSeed, BigInteger primeGenCounter)
        {
            Prime = prime;
            PrimeSeed = primeSeed;
            PrimeGenCounter = primeGenCounter;
        }

        public STRandomPrimeResult(string fail)
        {
            ErrorMessage = fail;
            Prime = 0;
            PrimeSeed = 0;
            PrimeGenCounter = 0;
        }

        public bool Success => (Prime != 0) && (PrimeSeed != 0) && (PrimeGenCounter != 0);
    }
}
