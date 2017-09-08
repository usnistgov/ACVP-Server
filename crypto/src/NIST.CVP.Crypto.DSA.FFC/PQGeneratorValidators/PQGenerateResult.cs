using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class PQGenerateResult
    {
        public BigInteger P { get; }
        public BigInteger Q { get; }
        public DomainSeed Seed { get; }
        public Counter Count { get; }
        public string ErrorMessage { get; }

        public PQGenerateResult(BigInteger p, BigInteger q, DomainSeed seed, Counter count)
        {
            P = p;
            Q = q;
            Seed = seed;
            Count = count;
        }

        public PQGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
