using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class PrimeGeneratorResult
    {
        public PrimePair Primes { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public PrimeGeneratorResult(PrimePair primes)
        {
            Primes = primes;
        }

        public PrimeGeneratorResult(string error)
        {
            ErrorMessage = error;
        }
    }
}
