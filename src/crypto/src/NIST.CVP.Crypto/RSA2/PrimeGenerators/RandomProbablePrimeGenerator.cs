using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class RandomProbablePrimeGenerator : PrimeGeneratorBase
    {
        public RandomProbablePrimeGenerator(IEntropyProvider entropyProvider, PrimeTestModes primeTestMode)
            : base(entropyProvider: entropyProvider, primeTestMode: primeTestMode) { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            var kat = _entropyProvider.GetType() == typeof(TestableEntropyProvider);

            // Remove these because we need this to support 1536-bit, 4096-bit and some smaller e values (3, 17)
            //// 1
            //if (nlen != 2048 && nlen != 3072)
            //{
            //    return new PrimeGeneratorResult("Incorrect nlen, must be 2048, 3072");
            //}

            //// 2
            //if (e <= BigInteger.Pow(2, 16) || e >= BigInteger.Pow(2, 256) || e.IsEven)
            //{
            //    return new PrimeGeneratorResult("Incorrect e, must be greater than 2^16, less than 2^256, odd");
            //}

            // 3
            // security_strength doesn't matter

            // 4, 4.1
            var i = 0;
            BigInteger p = 0;
            var bound = GetBound(nlen);
            do
            {
                do
                {
                    // 4.2
                    if (p != 0 && kat)
                    {
                        return new PrimeGeneratorResult("Given p less than sqrt(2) * 2 ^ (n/2) - 1, need to get a new random number.");
                    }
                    p = _entropyProvider.GetEntropy(nlen / 2).ToPositiveBigInteger();

                    // 4.3
                    if (p.IsEven)
                    {
                        p++;
                    }

                    // 4.4
                } while (p < bound);

                // 4.5
                if (NumberTheory.GCD(p - 1, e) == 1)
                {
                    if (MillerRabin(nlen, p, false))
                    {
                        break;
                    }
                }

                // 4.6, 4.7
                i++;
                if (i >= 5 * (nlen / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for p");
                }

                if (kat)
                {
                    return new PrimeGeneratorResult("Given p is not prime");
                }
            } while (!kat);

            // 5, 5.1
            i = 0;
            BigInteger q = 0;
            do
            {
                do
                {
                    // 5.2
                    if (q != 0 && kat)
                    {
                        return new PrimeGeneratorResult("Given q less than sqrt(2) * 2 ^ (n/2) - 1, need to get a new random number.");
                    }
                    q = _entropyProvider.GetEntropy(nlen / 2).ToPositiveBigInteger();

                    // 5.3
                    if (q.IsEven)
                    {
                        q++;
                    }

                    // 5.4
                    // 5.5
                } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100) || q < bound);

                // 5.6
                if (NumberTheory.GCD(q - 1, e) == 1)
                {
                    if (MillerRabin(nlen, q, false))
                    {
                        break;
                    }
                }

                // 5.7, 5.8
                i++;
                if (i >= 5 * (nlen / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for q");
                }

                if (kat)
                {
                    return new PrimeGeneratorResult("Given q is not prime");
                }
            } while (!kat);

            var auxValues = new AuxiliaryResult();
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }

        private BigInteger GetBound(int nlen)
        {
            switch (nlen)
            {
                case 1024:
                    return _root2Mult2Pow512Minus1;
                case 1536:
                    return _root2Mult2Pow768Minus1;
                case 2048:
                    return _root2Mult2Pow1024Minus1;
                case 3072:
                    return _root2Mult2Pow1536Minus1;
                case 4096:
                    return _root2Mult2Pow2048Minus1;
                default:
                    throw new ArgumentException("Invalid nlen");
            }
        }
    }
}
