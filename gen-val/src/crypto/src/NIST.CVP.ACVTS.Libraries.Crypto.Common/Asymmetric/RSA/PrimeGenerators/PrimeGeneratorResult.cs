namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public class PrimeGeneratorResult
    {
        public PrimePair Primes { get; }
        public AuxiliaryResult AuxValues { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public PrimeGeneratorResult(PrimePair primes, AuxiliaryResult aux)
        {
            Primes = primes;
            AuxValues = aux;
        }

        public PrimeGeneratorResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
