using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class ProvablePQGeneratorValidator : IPQGeneratorValidator
    {
        private readonly ISha _sha;
        private IEntropyProvider _entropy;
        private IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();

        public ProvablePQGeneratorValidator(ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            _sha = sha;
            _entropy = _entropyFactory.GetEntropyProvider(entropyType);
        }

        public void AddEntropy(BitString entropy)
        {
            _entropy.AddEntropy(entropy);
        }

        /// <summary>
        /// A.1.2.1.1 Get a first seed value
        /// </summary>
        /// <param name="N"></param>
        /// <param name="seedLen"></param>
        /// <returns></returns>
        private BigInteger GetFirstSeed(int L, int N, int seedLen)
        {
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return 0;
            }

            if (seedLen < N)
            {
                return 0;
            }

            BitString firstSeed;
            do
            {
                firstSeed = _entropy.GetEntropy(seedLen);
            } while (firstSeed.ToPositiveBigInteger() < NumberTheory.Pow2(N - 1));

            return firstSeed.ToPositiveBigInteger();
        }

        /// <summary>
        /// A.1.2.1.2 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="N"></param>
        /// <param name="seedLen"></param>
        /// <returns></returns>
        public PQGenerateResult Generate(int L, int N, int seedLen)
        {
            return Generate(L, N, GetFirstSeed(L, N, seedLen));
        }

        /// <summary>
        /// A.1.2.1.2 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="N"></param>
        /// <param name="firstSeed"></param>
        /// <returns></returns>
        private PQGenerateResult Generate(int L, int N, BigInteger firstSeed)
        {
            // 1
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new PQGenerateResult("Bad L, N pair");
            }

            // 2
            var qResult = PrimeGen186_4.ShaweTaylorRandomPrime(N, firstSeed, _sha);
            if (!qResult.Success)
            {
                return new PQGenerateResult("Failed to generate q from ShaweTaylor");
            }
            var q = qResult.Prime;
            var qSeed = qResult.PrimeSeed;
            var qCounter = qResult.PrimeGenCounter;

            // 3
            var pLen = L.CeilingDivide(2) + 1;
            var pResult = PrimeGen186_4.ShaweTaylorRandomPrime(pLen, qSeed, _sha);
            if (!pResult.Success)
            {
                return new PQGenerateResult("Failed to generate p0 from ShaweTaylor");
            }
            var p0 = pResult.Prime;
            var pSeed = pResult.PrimeSeed;
            var pCounter = pResult.PrimeGenCounter;

            // 4, 5
            var outLen = _sha.HashFunction.OutputLen;
            var iterations = L.CeilingDivide(outLen) - 1;
            var oldCounter = pCounter;

            // 6, 7
            BigInteger x = 0;
            for (var i = 0; i <= iterations; i++)
            {
                x += _sha.HashNumber(pSeed + i).ToBigInteger() * NumberTheory.Pow2(i * outLen);
            }

            // 8
            pSeed += iterations + 1;

            // 9
            x = NumberTheory.Pow2(L - 1) + (x % NumberTheory.Pow2(L - 1));

            // 10
            var t = x.CeilingDivide(2 * q * p0);

            do
            {
                // 11
                if (2 * t * q * p0 + 1 > NumberTheory.Pow2(L))
                {
                    t = NumberTheory.Pow2(L - 1).CeilingDivide(2 * q * p0);
                }

                // 12, 13
                var p = 2 * t * q * p0 + 1;
                pCounter++;

                // 14, 15
                BigInteger a = 0;
                for (var i = 0; i <= iterations; i++)
                {
                    a += _sha.HashNumber(pSeed + i).ToBigInteger() * NumberTheory.Pow2(i * outLen);
                }

                // 16
                pSeed += iterations + 1;

                // 17
                a = 2 + (a % (p - 3));

                // 18
                var z = BigInteger.ModPow(a, 2 * t * q, p);

                // 19
                if (1 == NumberTheory.GCD(z - 1, p) && 1 == BigInteger.ModPow(z, p0, p))
                {
                    return new PQGenerateResult(p, q, new DomainSeed(firstSeed, pSeed, qSeed), new Counter(pCounter, qCounter));
                }

                // 20
                if (pCounter > 4 * L + oldCounter)
                {
                    return new PQGenerateResult("Too many iterations");
                }

                // 21
                t++;

                // 22
            } while (true);
        }

        /// <summary>
        /// A.1.2.2
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="seed"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count)
        {
            // 0, domain type check
            if (seed.Mode != PrimeGenMode.Provable && count.Mode != PrimeGenMode.Provable)
            {
                return new PQValidateResult("Invalid DomainSeed and Counter");
            }

            // 1, 2
            var L = new BitString(p).BitLength;
            var N = new BitString(q).BitLength;

            // 3
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new PQValidateResult("Invalid L, N pair");
            }

            // 4
            if (seed.Seed < NumberTheory.Pow2(N - 1))
            {
                return new PQValidateResult("Bad first seed");
            }

            // 5
            if (NumberTheory.Pow2(N) <= q)
            {
                return new PQValidateResult("Bad q, too small");
            }

            // 6
            if (NumberTheory.Pow2(L) <= p)
            {
                return new PQValidateResult("Bad p, too large");
            }

            // 7
            if ((p - 1) % q != 0)
            {
                return new PQValidateResult("p - 1 % q != 0, bad values");
            }

            // 8
            var computed_result = Generate(L, N, seed.Seed);
            if (!computed_result.Success)
            {
                return new PQValidateResult("Failed to generate p and q");
            }

            if (q != computed_result.Q || seed.QSeed != computed_result.Seed.QSeed || count.QCount != computed_result.Count.QCount)
            {
                return new PQValidateResult("Failed to generate given q");
            }

            if (p != computed_result.P || seed.PSeed != computed_result.Seed.PSeed || count.PCount != computed_result.Count.PCount)
            {
                return new PQValidateResult("Failed to generate given p");
            }

            return new PQValidateResult();
        }
    }
}
