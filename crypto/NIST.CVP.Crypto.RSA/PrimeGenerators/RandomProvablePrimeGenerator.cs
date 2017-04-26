using System.Numerics;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    internal struct PPCResult
    {
        public bool Status;
        public BigInteger P, P1, P2, PSeed;
        public string ErrorMessage;

        public PPCResult(string fail)
        {
            ErrorMessage = fail;
            Status = false;
            P = P1 = P2 = PSeed = 0;
        }

        public PPCResult(BigInteger p, BigInteger p1, BigInteger p2, BigInteger pSeed)
        {
            Status = true;
            P = p;
            P1 = p1;
            P2 = p2;
            PSeed = pSeed;
            ErrorMessage = "";
        }
    }

    public class RandomProvablePrimeGenerator : PrimeGeneratorBase
    {
        public RandomProvablePrimeGenerator(HashFunction hashFunction) : base(hashFunction) { }
        public RandomProvablePrimeGenerator() : base() { }

        /// <summary>
        /// B.3.2 Generate Random Provable Primes
        /// </summary>
        /// <param name="nlen"></param>
        /// <param name="e"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            // 1
            if (nlen != 2048 && nlen != 3072)
            {
                return new PrimeGeneratorResult("Incorrect nlen, must be 2048, 3072");
            }

            // 2
            if (e <=  NumberTheory.Pow2(16) || e >= NumberTheory.Pow2(256) || e.IsEven)
            {
                return new PrimeGeneratorResult("Incorrect e, must be greater than 2^16, less than 2^256, odd");
            }

            // 3
            int security_strength;
            if (nlen == 2048)
            {
                security_strength = 112;
            }
            else // if (nlen == 3072)
            {
                security_strength = 128;
            }
            
            // 4
            if (seed.BitLength != 2 * security_strength)
            {
                return new PrimeGeneratorResult("Incorrect seed length");
            }

            // 5
            var workingSeed = seed.ToPositiveBigInteger();

            // 6
            var ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
            if (!ppcResult.Status)
            {
                return new PrimeGeneratorResult($"Bad Provable Prime Construction for p: {ppcResult.ErrorMessage}");
            }
            var p = ppcResult.P;
            workingSeed = ppcResult.PSeed;

            BigInteger q = 0;
            while (true)
            {
                // 7
                ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
                if (!ppcResult.Status)
                {
                    return new PrimeGeneratorResult($"Bad Provable Prime Construction for q: {ppcResult.ErrorMessage}");
                }
                q = ppcResult.P;
                workingSeed = ppcResult.PSeed;

                // 8
                if (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100))
                {
                    break;
                }
            }

            // 9, 10
            return new PrimeGeneratorResult(p, q);
        }

        /// <summary>
        /// C.10 Provable Prime Construction
        /// </summary>
        /// <param name="L"></param>
        /// <param name="N1"></param>
        /// <param name="N2"></param>
        /// <param name="firstSeed"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private PPCResult ProvablePrimeConstruction(int L, int N1, int N2, BigInteger firstSeed, BigInteger e)
        {
            // 1
            if (N1 + N2 > L - System.Math.Ceiling(L / 2.0) - 4)
            {
                return new PPCResult("PPC: fail N1 + N2 check");
            }

            BigInteger p, p0, p1, p2;
            BigInteger pSeed, p0Seed, p2Seed;

            // 2
            if (N1 == 1)
            {
                p1 = 1;
                p2Seed = firstSeed;
            }

            // 3
            if (N1 >= 2)
            {
                var stResult = ShaweTaylorRandomPrime(N1, firstSeed);
                if (!stResult.Success)
                {
                    return new PPCResult("PPC: fail ST p1 gen");
                }

                p1 = stResult.Prime;
                p2Seed = stResult.PrimeSeed;
            }

            // 4
            if (N2 == 1)
            {
                p2 = 1;
                p0Seed = p2Seed;
            }

            // 5
            if (N2 >= 2)
            {
                var stResult = ShaweTaylorRandomPrime(N2, p2Seed);
                if (!stResult.Success)
                {
                    return new PPCResult("PPC: fail ST p2 gen");
                }

                p2 = stResult.Prime;
                p0Seed = stResult.PrimeSeed;
            }

            // 6
            var result = ShaweTaylorRandomPrime((int)System.Math.Ceiling(L / 2.0) + 1, p0Seed);
            if (!result.Success)
            {
                return new PPCResult("PPC: fail ST p0 gen");
            }

            p0 = result.Prime;
            pSeed = result.PrimeSeed;

            // 7, 8, 9
            var outLen = GetOutLen();
            var iterations = NumberTheory.CeilingDivide(L, outLen) - 1;
            var pGenCounter = 0;
            BigInteger x = 0;

            // 10
            for (var i = 0; i < iterations; i++)
            {
                x += Hash(pSeed + i) * NumberTheory.Pow2(i * outLen);
            }

            // 11
            pSeed += iterations + 1;

            // 12
            // sqrt(2) * 2^(L-1)
            var lowerBound = L == 1536 ? _root2Mult2Pow1536Minus1 : _root2Mult2Pow1024Minus1;
            x = lowerBound + x % (NumberTheory.Pow2(L - 1) - lowerBound);

            // 13
            if (NumberTheory.GCD(p0 * p1, p2) != 1)
            {
                return new PPCResult("PPC: GCD fail for p0 * p1 and p2");
            }

            // 14
            var y = NumberTheory.ModularInverse(p0 * p1, p2);
            if (y == 0)
            {
                y = p2;
            }

            // 15
            var t = NumberTheory.CeilingDivide(2 * y * p0 * p1 + x, 2 * p0 * p1 * p2);

            while (true)
            {
                // 16
                if (2 * (t * p2 - y) * p0 * p1 + 1 > NumberTheory.Pow2(L))
                {
                    t = NumberTheory.CeilingDivide(2 * y * p0 * p1 + lowerBound, 2 * p0 * p1 * p2);
                }

                // 17, 18
                p = 2 * (t * p2 - y) * p0 * p1 + 1;
                pGenCounter++;

                // 19
                if (NumberTheory.GCD(p - 1, e) == 1)
                {
                    BigInteger a = 0;
                    for (var i = 0; i < iterations; i++)
                    {
                        a += Hash(pSeed + i) * NumberTheory.Pow2(i * outLen);
                    }

                    pSeed += iterations + 1;
                    a = 2 + a % (p - 3);
                    var z = BigInteger.ModPow(a, 2 * (t * p2 - y) * p1, p);

                    if (NumberTheory.GCD(z - 1, p) == 1 && 1 == BigInteger.ModPow(z, p0, p))
                    {
                        return new PPCResult(p, p1, p2, pSeed);
                    }
                }

                // 20
                if (pGenCounter >= 5 * L)
                {
                    return new PPCResult("PPC: too many iterations");
                }

                // 21
                t++;

                // 22 (loop)
            }
        }
    }
}
