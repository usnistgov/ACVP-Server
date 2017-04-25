using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    internal struct PPCResult
    {
        public bool Status;
        public BigInteger P, P1, P2, PSeed;

        public PPCResult(string fail)
        {
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
        }
    }

    // B.3.2 
    public class RandomProvablePrimeGenerator : PrimeGeneratorBase
    {
        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            // 1
            if (nlen != 2048 || nlen != 3072)
            {
                return new PrimeGeneratorResult("Incorrect nlen, must be 2048, 3072");
            }

            // 2
            if (e <= BigInteger.Pow(2, 16) || e >= BigInteger.Pow(2, 256) || e.IsEven)
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
            var workingSeed = seed.ToBigInteger();

            // 6
            var ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
            if (!ppcResult.Status)
            {
                return new PrimeGeneratorResult("Bad Provable Prime Construction for p");
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
                    return new PrimeGeneratorResult("Bad Provable Prime Construction for q");
                }
                q = ppcResult.P;
                workingSeed = ppcResult.PSeed;

                // 8
                if (BigInteger.Abs(p - q) <= BigInteger.Pow(2, nlen / 2 - 100))
                {
                    break;
                }
            }

            // 9, 10
            return new PrimeGeneratorResult(p, q);
        }

        // C.10
        private PPCResult ProvablePrimeConstruction(int L, int N1, int N2, BigInteger firstSeed, BigInteger e)
        {
            // 1
            if (N1 + N2 > L - System.Math.Ceiling(L / 2.0) - 4)
            {
                return new PPCResult("fail");
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
                    return new PPCResult("fail");
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
                    return new PPCResult("fail");
                }

                p2 = stResult.Prime;
                p0Seed = stResult.PrimeSeed;
            }

            // 6
            var result = ShaweTaylorRandomPrime((int)System.Math.Ceiling(L / 2.0) + 1, p0Seed);
            if (!result.Success)
            {
                return new PPCResult("fail");
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
                x += Hash(pSeed + i) * (BigInteger)System.Math.Pow(2, i * outLen);
            }

            // 11
            pSeed += iterations + 1;

            // 12
            var lowerBound = (BigInteger) System.Math.Sqrt(2) * BigInteger.Pow(2, L - 1);
            x = lowerBound + x % (BigInteger.Pow(2, L - 1) - lowerBound);

            // 13
            if (NumberTheory.GCD(p0 * p1, p2) != 1)
            {
                return new PPCResult("fail");
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
                if (2 * (t * p2 - y) * p0 * p1 + 1 > BigInteger.Pow(2, L))
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
                        a += Hash(pSeed + i) * BigInteger.Pow(2, i * outLen);
                    }

                    pSeed += iterations + 1;
                    a = 2 + a % (p - 3);
                    var z = NumberTheory.Pow(a, 2 * (t * p2 - y) * p1) % p;

                    if (NumberTheory.GCD(z - 1, p) == 1 && 1 == NumberTheory.Pow(z, p0) % p)
                    {
                        return new PPCResult(p, p1, p2, pSeed);
                    }
                }

                // 20
                if (pGenCounter >= 5 * L)
                {
                    return new PPCResult("fail");
                }

                // 21
                t++;

                // 22 (loop)
            }
        }
    }
}
