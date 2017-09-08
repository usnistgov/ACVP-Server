using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
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

                // Check if q is prime, if not go back to 5
                } while (false);

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
                        if (true)
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

        public PQValidateResult Validate(BigInteger p, BigInteger q, DomainSeed seed, Counter count)
        {
            throw new NotImplementedException();
        }
    }
}
