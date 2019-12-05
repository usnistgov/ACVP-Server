using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class PpfResult
    {
        public BigInteger Prime { get; }
        public BigInteger XPrime { get; }
        public string ErrorMessage { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public PpfResult(string fail)
        {
            ErrorMessage = fail;
        }

        public PpfResult(BigInteger p, BigInteger xp)
        {
            Prime = p;
            XPrime = xp;
        }
    }
}
