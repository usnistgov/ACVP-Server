using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.Crypto.RSA
{
    public class STRandomPrimeResult
    {
        public BigInteger Prime { get; private set; }
        public BigInteger PrimeSeed { get; private set; }
        public BigInteger PrimeGenCounter { get; private set; }

        public STRandomPrimeResult(BigInteger prime, BigInteger primeSeed, BigInteger primeGenCounter)
        {
            Prime = prime;
            PrimeSeed = primeSeed;
            PrimeGenCounter = primeGenCounter;
        }

        public STRandomPrimeResult(string fail)
        {
            Prime = 0;
            PrimeSeed = 0;
            PrimeGenCounter = 0;
        }

        public bool Success => (Prime != 0) && (PrimeSeed != 0) && (PrimeGenCounter != 0);
    }
}
