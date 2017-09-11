using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC.PQGeneratorValidators
{
    public class ProbablePQGeneratorValidator : IPQGeneratorValidator
    {
        private readonly ISha _sha;
        private readonly IRandom800_90 _rand = new Random800_90();

        public ProbablePQGeneratorValidator(ISha sha)
        {
            _sha = sha;
        }

        /// <summary>
        /// A.1.1.2 from FIPS 186-4
        /// </summary>
        /// <param name="L"></param>
        /// <param name="N"></param>
        /// <param name="seedLen"></param>
        /// <returns></returns>
        public PQGenerateResult Generate(int L, int N, int seedLen)
        {
            // 1. Check L/N pair
            if (!DSAHelper.VerifyLenPair(L, N))
            {
                return new PQGenerateResult("Invalid L, N pair");
            }

            // 2. Check seedLen
            if (seedLen < N)
            {
                return new PQGenerateResult("Invalid seedLen");
            }

            // 3, 4 Compute n, b
            var outLen = _sha.HashFunction.OutputLen;
            var n = (int)NumberTheory.CeilingDivide(L, outLen) - 1;
            var b = L - 1 - (n * outLen);

            do
            {
                BigInteger seed, q;

                do
                {
                    // 5. Get random seed
                    seed = _rand.GetRandomBitString(seedLen).ToPositiveBigInteger();

                    // 6. Hash seed
                    var U = _sha.HashNumber(seed).ToBigInteger() % NumberTheory.Pow2(N - 1);

                    // 7. Compute q
                    q = NumberTheory.Pow2(N - 1) + U - 1 - (U % 2);

                // Check if q is prime, if not go back to 5, assume highest security strength
                } while (!NumberTheory.MillerRabin(q, 64));

                // 10, 11 Compute p
                var offset = 1;
                for (var ctr = 0; ctr <= (4 * L - 1); ctr++)
                {
                    // 11.1
                    var V = new List<BigInteger>();
                    for (var j = 0; j <= n; j++)
                    {
                        V.Add(_sha.HashNumber(seed + offset + j).ToBigInteger());
                    }

                    // 11.2
                    var W = V[0];
                    for (var j = 1; j < n; j++)
                    {
                        W += V[1] * NumberTheory.Pow2(outLen);
                    }
                    W += V[n] % NumberTheory.Pow2(b) * NumberTheory.Pow2(n * outLen);

                    // 11.3
                    var X = W + NumberTheory.Pow2(L - 1);

                    // 11.4
                    var c = X % (2 * q);

                    // 11.5
                    var p = X - (c - 1);

                    // 11.6, 11.7, 11.8
                    if (p >= NumberTheory.Pow2(L - 1))
                    {
                        // Check if p is prime, if so return
                        if (NumberTheory.MillerRabin(p, 64))
                        {
                            return new PQGenerateResult(p, q, new DomainSeed(seed), new Counter(ctr));
                        }
                    }

                    // 11.9
                    offset += n + 1;
                }

            // 12
            } while (true);
        }

        /// <summary>
        /// A.1.1.3 from FIPS 186-4
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="seed"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count)
        {
            if (seed.Mode != PrimeGenMode.Probable && count.Mode != PrimeGenMode.Probable)
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
            if (count.GetCounter() > (4 * L - 1))
            {
                return new PQValidateResult("Invalid counter");
            }

            // 5, 6
            var seedLen = new BitString(seed.GetSeed()).BitLength;
            if (seedLen < N)
            {
                return new PQValidateResult("Invalid seed");
            }

            // 7
            var U = _sha.HashNumber(seed.GetSeed()).ToBigInteger() % NumberTheory.Pow2(N - 1);

            // 8
            var computed_q = NumberTheory.Pow2(N - 1) + U + 1 - (U % 2);

            // 9
            if (!NumberTheory.MillerRabin(computed_q, 64) || computed_q != q)
            {
                return new PQValidateResult("Q not prime, or doesn't match expected value");
            }

            // 10, 11, 12
            var outLen = _sha.HashFunction.OutputLen;
            var n = (int)NumberTheory.CeilingDivide(L, outLen) - 1;
            var b = L - 1 - (n * outLen);
            var offset = 1;

            // 13
            BigInteger computed_p;
            int i;
            for (i = 0; i <= count.GetCounter(); i++)
            {
                // 13.1
                var V = new List<BigInteger>();
                for (var j = 0; j <= n; j++)
                {
                    V.Add(_sha.HashNumber(seed.GetSeed() + offset + j).ToBigInteger() % NumberTheory.Pow2(seedLen));
                }

                // 13.2
                var W = V[0];
                for (var j = 1; j < n; j++)
                {
                    W += V[1] * NumberTheory.Pow2(outLen);
                }
                W += V[n] % NumberTheory.Pow2(b) * NumberTheory.Pow2(n * outLen);

                // 13.3
                var X = W + NumberTheory.Pow2(L - 1);

                // 13.4
                var c = X % (2 * q);

                // 13.5
                computed_p = X - (c - 1);

                // 13.6, 13.7, 13.8
                if (computed_p >= NumberTheory.Pow2(L - 1))
                {
                    // Check if p is prime, if so return
                    if (NumberTheory.MillerRabin(computed_p, 64))
                    {
                        break;
                    }
                }

                // 13.9
                offset += n + 1;
            }

            // 14
            if (i != count.GetCounter() || computed_p != p || NumberTheory.MillerRabin(computed_p, 64))
            {
                return new PQValidateResult("Invalid p value or counter");
            }

            // 15
            return new PQValidateResult();
        }
    }
}
