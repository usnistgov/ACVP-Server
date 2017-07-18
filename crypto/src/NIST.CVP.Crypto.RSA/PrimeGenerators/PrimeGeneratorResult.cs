using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class PrimeGeneratorResult
    {
        public BigInteger P { get; private set; }
        public BigInteger Q { get; private set; }
        public AuxiliaryPrimeGeneratorResult AuxValues { get; private set; } = null;
        public string ErrorMessage { get; private set; }

        // B32, B33, B34
        public PrimeGeneratorResult(BigInteger p, BigInteger q)
        {
            P = p;
            Q = q;
        }

        // B35, B36
        public PrimeGeneratorResult(BigInteger p, BigInteger q, AuxiliaryPrimeGeneratorResult aux)
        {
            P = p;
            Q = q;
            AuxValues = aux;
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
