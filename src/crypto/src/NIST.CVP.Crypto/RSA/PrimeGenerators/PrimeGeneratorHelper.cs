using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public static class PrimeGeneratorHelper
    {
        public static readonly BigInteger Root2Mult2Pow512Minus1 =  new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow768Minus1 =  new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow1024Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow1536Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A81394AB6D8FD0EFDF4D3A02CEBC93E0C4264DABCD528B651B8CF341B6F8236C70104DC01FE32352F332A5E9F7BDA1EBFF6A1BE3FCA221307DEA06241F7AA81C2").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow2048Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A81394AB6D8FD0EFDF4D3A02CEBC93E0C4264DABCD528B651B8CF341B6F8236C70104DC01FE32352F332A5E9F7BDA1EBFF6A1BE3FCA221307DEA06241F7AA81C2C1FCBDDEA2F7DC3318838A2EAFF5F3B2D24F4A763FACB882FDFE170FD3B1F780F9ACCE41797F2805C246785E929570235FCF8F7BCA3EA33B4D7C60A5E633E3E1").ToPositiveBigInteger();
        
        private static readonly BigInteger _2Pow512MinusFloorRoot2Mult2Pow1024Minus1 =  new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D898").ToPositiveBigInteger();
        private static readonly BigInteger _2Pow1024MinusFloorRoot2Mult2Pow1024Minus1 = new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D8972DDFD178BD50E0B1ACFA639FEE43CC84354E436EE977BA75B9F5438DD083B1CC392A575C7448162334D59CBCCE0C37B20AD0EDF07C91A7D1155B5F766FBF35B6").ToPositiveBigInteger();
        private static readonly BigInteger _2Pow1536MinusFloorRoot2Mult2Pow1536Minus1 = new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D8972DDFD178BD50E0B1ACFA639FEE43CC84354E436EE977BA75B9F5438DD083B1CC392A575C7448162334D59CBCCE0C37B20AD0EDF07C91A7D1155B5F766FBF35B57EC6B5492702F1020B2C5FD31436C1F3BD9B25432AD749AE4730CBE4907DC938FEFB23FE01CDCAD0CCD5A1608425E140095E41C035DDECF8215F9DBE08557E3E").ToPositiveBigInteger();

        private static readonly List<(PrimeTestModes primeTest, int nlen, bool auxPrime, int iterations)> _mrIterations
            = new List<(PrimeTestModes primeTest, int nlen, bool auxPrime, int iterations)>
            {
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 1024, true, 28),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 1024, false, 5),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 2048, true, 38),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 2048, false, 5),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 3072, true, 41),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 3072, false, 4),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 4096, true, 44),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 4096, false, 4),

                (PrimeTestModes.TwoPow100ErrorBound, 1024, true, 38),
                (PrimeTestModes.TwoPow100ErrorBound, 1024, false, 7),
                (PrimeTestModes.TwoPow100ErrorBound, 1536, false, 4),    // 1536 only used with 2^100 bound, with no aux primes
                (PrimeTestModes.TwoPow100ErrorBound, 2048, true, 32),
                (PrimeTestModes.TwoPow100ErrorBound, 2048, false, 4),
                (PrimeTestModes.TwoPow100ErrorBound, 3072, true, 27),
                (PrimeTestModes.TwoPow100ErrorBound, 3072, false, 3),
                (PrimeTestModes.TwoPow100ErrorBound, 4096, true, 22),
                (PrimeTestModes.TwoPow100ErrorBound, 4096, false, 2)
            };
        
        public static int GetSecurityStrengthFromModulus(int modulus)
        {
            switch (modulus)
            {
                case 1024:        // limited support
                    return 80;
                
                case 2048:
                    return 112;
                
                case 3072:
                    return 128;
                
                case 4096:
                    return 128;
                
                default:
                    throw new ArgumentException($"{nameof(modulus)} provided is invalid: {modulus}.");
            }
        }
        
        /// <summary>
        /// C.9 Compute a Probable Prime Factor Based on Auxiliary Primes
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="nLen"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static PpfResult ProbablePrimeFactor(PrimeTestModes primeTestMode, IEntropyProvider entropyProvider, int pBound, int c, BigInteger r1, BigInteger r2, int nLen, BigInteger e)
        {
            BigInteger lowerBound, upperBound;
            upperBound = NumberTheory.Pow2(nLen / 2) - 1;
            switch (nLen)
            {
                case 1024:
                    lowerBound = Root2Mult2Pow512Minus1;
                    break;
                case 2048:
                    lowerBound = Root2Mult2Pow1024Minus1;
                    break;
                case 3072:
                    lowerBound = Root2Mult2Pow1536Minus1;
                    break;
                case 4096:
                    lowerBound = Root2Mult2Pow2048Minus1;
                    break;
                
                default:
                    return new PpfResult("Invalid nLen provided.");
            }

            // 1
            if (NumberTheory.GCD(2 * r1, r2) != 1)
            {
                return new PpfResult("PPF: GCD requirement not met");
            }

            // 2 and ensure R is positive
            var Rfirst = r2.ModularInverse(2 * r1) * r2;
            var Rsecond = (2 * r1).ModularInverse(r2) * 2 * r1;

            BigInteger R;
            do
            {
                if (Rfirst < Rsecond)
                {
                    Rfirst += (2 * r1 * r2);
                }
                R = Rfirst - Rsecond;
            } while (Rfirst < Rsecond);

            while (true)
            {
                var modulo = 2 * r1 * r2;

                BigInteger x, y;
                int i;
                
                do
                {
                    // 3
                    x = entropyProvider.GetEntropy(lowerBound, upperBound);

                    // 4
                    y = x + (R - x).PosMod(modulo);

                    // 4.1 (FIPS 186-5)
                    if (c != 0)
                    {
                        // This ends after a maximum of 4 iterations
                        while (y % 8 != c)
                        {
                            y += modulo;
                        }
                    }
                    
                    // 5
                    i = 0;

                    // 6
                } while (y >= NumberTheory.Pow2(nLen / 2));

                do
                {
                    // 7
                    if (NumberTheory.GCD(y - 1, e) == 1)
                    {
                        if (MillerRabin(primeTestMode, nLen, y, true))
                        {
                            return new PpfResult(y, x);
                        }
                    }

                    // 8
                    i++;

                    // 9
                    if (i >= pBound * (nLen / 2))
                    {
                        return new PpfResult("PPF: Too many iterations");
                    }

                    // 10
                    if (c == 0)
                    {
                        y += modulo;
                    }
                    else
                    {
                        y += modulo * 4;
                    }

                    // 6 (repeated)
                } while (y < NumberTheory.Pow2(nLen / 2));
            }
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
        public static PpcResult ProvablePrimeConstruction(ISha sha, int L, int N1, int N2, BigInteger firstSeed, BigInteger e)
        {
            // 1
            if (N1 + N2 > L - System.Math.Ceiling(L / 2.0) - 4)
            {
                return new PpcResult("PPC: fail N1 + N2 check");
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
                var stResult = PrimeGen186_4.ShaweTaylorRandomPrime(N1, firstSeed, sha);
                if (!stResult.Success)
                {
                    return new PpcResult("PPC: fail ST p1 gen");
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
                var stResult = PrimeGen186_4.ShaweTaylorRandomPrime(N2, p2Seed, sha);
                if (!stResult.Success)
                {
                    return new PpcResult("PPC: fail ST p2 gen");
                }

                p2 = stResult.Prime;
                p0Seed = stResult.PrimeSeed;
            }

            // 6
            var result = PrimeGen186_4.ShaweTaylorRandomPrime((int)System.Math.Ceiling(L / 2.0) + 1, p0Seed, sha);
            if (!result.Success)
            {
                return new PpcResult("PPC: fail ST p0 gen");
            }

            p0 = result.Prime;
            pSeed = result.PrimeSeed;

            // 7, 8, 9
            var outLen = sha.HashFunction.OutputLen;
            var iterations = L.CeilingDivide(outLen) - 1;
            var pGenCounter = 0;
            BigInteger x = 0;

            // 10
            for (var i = 0; i <= iterations; i++)
            {
                x += sha.HashNumber(pSeed + i).ToBigInteger() * NumberTheory.Pow2(i * outLen);
            }

            // 11
            pSeed += iterations + 1;

            // 12
            // sqrt(2) * 2^(L-1)
            BigInteger lowerBound;
            BigInteger modulo;

            if (L == 1024 / 2)
            {
                lowerBound = Root2Mult2Pow512Minus1;
                modulo = _2Pow512MinusFloorRoot2Mult2Pow1024Minus1;
            }
            else if (L == 2048 / 2)
            {
                lowerBound = Root2Mult2Pow1024Minus1;
                modulo = _2Pow1024MinusFloorRoot2Mult2Pow1024Minus1;
            }
            else if (L == 3072 / 2)
            {
                lowerBound = Root2Mult2Pow1536Minus1;
                modulo = _2Pow1536MinusFloorRoot2Mult2Pow1536Minus1;
            }
            else if (L == 4096 / 2)
            {
                lowerBound = Root2Mult2Pow2048Minus1;
                modulo = NumberTheory.Pow2(2048) - Root2Mult2Pow2048Minus1; // TODO need to confirm value for 2048
            }

            x = lowerBound + x % modulo;

            // 13
            if (NumberTheory.GCD(p0 * p1, p2) != 1)
            {
                return new PpcResult("PPC: GCD fail for p0 * p1 and p2");
            }

            // 14
            var y = (p0 * p1).ModularInverse(p2);
            if (y == 0)
            {
                y = p2;
            }

            // 15
            var t = ((2 * y * p0 * p1) + x).CeilingDivide(2 * p0 * p1 * p2);

            while (true)
            {
                // 16
                if (2 * (t * p2 - y) * p0 * p1 + 1 > NumberTheory.Pow2(L))
                {
                    t = ((2 * y * p0 * p1) + lowerBound).CeilingDivide(2 * p0 * p1 * p2);
                }

                // 17, 18
                p = 2 * (t * p2 - y) * p0 * p1 + 1;
                pGenCounter++;

                // 19
                if (NumberTheory.GCD(p - 1, e) == 1)
                {
                    BigInteger a = 0;
                    for (var i = 0; i <= iterations; i++)
                    {
                        a += sha.HashNumber(pSeed + i).ToBigInteger() * NumberTheory.Pow2(i * outLen);
                    }

                    pSeed += iterations + 1;
                    a = 2 + (a % (p - 3));
                    var z = BigInteger.ModPow(a, 2 * (t * p2 - y) * p1, p);

                    if (NumberTheory.GCD(z - 1, p) == 1 && 1 == BigInteger.ModPow(z, p0, p))
                    {
                        return new PpcResult(p, p1, p2, pSeed);
                    }
                }

                // 20
                if (pGenCounter >= 5 * L)
                {
                    return new PpcResult("PPC: too many iterations");
                }

                // 21
                t++;

                // 22 (loop)
            }
        }
        
        /// <summary>
        /// Miller-Rabin driver to determine how to properly call underlying function
        /// </summary>
        /// <param name="nlen"></param>
        /// <param name="val"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static bool MillerRabin(PrimeTestModes primeTestMode, int nlen, BigInteger val, bool auxPrime)
        {
            if (_mrIterations.TryFirst(tuple => tuple.primeTest == primeTestMode && tuple.nlen == nlen && tuple.auxPrime == auxPrime,
                out var matchedTuple))
            {
                return NumberTheory.MillerRabin(val, matchedTuple.iterations);
            }
            
            throw new ArgumentException($"Invalid parameters provided, unable to find iterations for MR");
        }
    }
}