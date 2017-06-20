using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class PrimeGeneratorResult
    {
        public BigInteger P { get; private set; }
        public BigInteger P1 { get; private set; } = 0;
        public BigInteger P2 { get; private set; } = 0;
        public BigInteger Q { get; private set; }
        public BigInteger Q1 { get; private set; } = 0;
        public BigInteger Q2 { get; private set; } = 0;

        public string ErrorMessage { get; private set; }

        public PrimeGeneratorResult(BigInteger p, BigInteger q)
        {
            P = p;
            Q = q;
        }

        public PrimeGeneratorResult(BigInteger p, BigInteger q, BigInteger p1, BigInteger q1, BigInteger p2, BigInteger q2)
        {
            P = p;
            Q = q;
            P1 = p1;
            Q1 = q1;
            P2 = p2;
            Q2 = q2;
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

            return $"P: {P}, Q: {Q}";
        }
    }
}
