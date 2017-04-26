using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class PrimeGeneratorResult
    {
        public BigInteger P { get; private set; }
        public BigInteger Q { get; private set; }
        public string ErrorMessage { get; private set; }

        public PrimeGeneratorResult(BigInteger p, BigInteger q)
        {
            P = p;
            Q = q;
        }

        public PrimeGeneratorResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }

            return $"P : {P}, Q: {Q}";
        }
    }
}
