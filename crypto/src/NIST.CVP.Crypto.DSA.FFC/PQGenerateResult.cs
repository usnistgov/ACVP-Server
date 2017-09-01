using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class PQGenerateResult
    {
        public BigInteger P { get; }
        public BigInteger Q { get; }
        public BigInteger Seed { get; }
        public int Counter { get; }
        public string ErrorMessage { get; }

        public PQGenerateResult(BigInteger p, BigInteger q, BigInteger seed, int counter)
        {
            P = p;
            Q = q;
            Seed = seed;
            Counter = counter;
        }

        public PQGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
