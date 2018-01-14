using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public class PrimeGeneratorResult
    {
        public BigInteger P { get; }
        public BigInteger Q { get; }
        public AuxiliaryPrimeGeneratorResult AuxValues { get; }
        public string ErrorMessage { get; }

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

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

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
