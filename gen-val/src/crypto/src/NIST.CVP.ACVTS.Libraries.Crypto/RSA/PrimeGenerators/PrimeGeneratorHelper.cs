using System;
using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Crypto.Math;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.PrimeGenerators
{
    public static class PrimeGeneratorHelper
    {
        public static readonly BigInteger Root2Mult2Pow512Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow768Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow1024Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow1536Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A81394AB6D8FD0EFDF4D3A02CEBC93E0C4264DABCD528B651B8CF341B6F8236C70104DC01FE32352F332A5E9F7BDA1EBFF6A1BE3FCA221307DEA06241F7AA81C2").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow2048Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A81394AB6D8FD0EFDF4D3A02CEBC93E0C4264DABCD528B651B8CF341B6F8236C70104DC01FE32352F332A5E9F7BDA1EBFF6A1BE3FCA221307DEA06241F7AA81C2C1FCBDDEA2F7DC3318838A2EAFF5F3B2D24F4A763FACB882FDFE170FD3B1F780F9ACCE41797F2805C246785E929570235FCF8F7BCA3EA33B4D7C60A5E633E3E1").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow3072Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A81394AB6D8FD0EFDF4D3A02CEBC93E0C4264DABCD528B651B8CF341B6F8236C70104DC01FE32352F332A5E9F7BDA1EBFF6A1BE3FCA221307DEA06241F7AA81C2C1FCBDDEA2F7DC3318838A2EAFF5F3B2D24F4A763FACB882FDFE170FD3B1F780F9ACCE41797F2805C246785E929570235FCF8F7BCA3EA33B4D7C60A5E633E3E1485F3B494D82BC6085AC27DA43E4927ADB8FC16E69481B04EF744894C1EA75568775190FBA44FA353F48185F107DBB4A77DAC64CC266EB850ED4822E1E899D034211EB71C181EC80DD4ED1A3B3423CB62E6ACB96E07F9AA061A094A16B203080F7B7E36F488A515A79246344E3005DA0545AB5820FEAEF3706E86336A418FF3F").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow4096Minus1 = new BitString("b504f333f9de6484597d89b3754abe9f1d6f60ba893ba84ced17ac85833399154afc83043ab8a2c3a8b1fe6fdc83db390f74a85e439c7b4a780487363dfa2768d2202e8742af1f4e53059c6011bc337bcab1bc911688458a460abc722f7c4e33c6d5a8a38bb7e9dccb2a634331f3c84df52f120f836e582eeaa4a0899040ca4a81394ab6d8fd0efdf4d3a02cebc93e0c4264dabcd528b651b8cf341b6f8236c70104dc01fe32352f332a5e9f7bda1ebff6a1be3fca221307dea06241f7aa81c2c1fcbddea2f7dc3318838a2eaff5f3b2d24f4a763facb882fdfe170fd3b1f780f9acce41797f2805c246785e929570235fcf8f7bca3ea33b4d7c60a5e633e3e1485f3b494d82bc6085ac27da43e4927adb8fc16e69481b04ef744894c1ea75568775190fba44fa353f48185f107dbb4a77dac64cc266eb850ed4822e1e899d034211eb71c181ec80dd4ed1a3b3423cb62e6acb96e07f9aa061a094a16b203080f7b7e36f488a515a79246344e3005da0545ab5820feaef3706e86336a418ff3fffababf23884c066deae134242ed2f48d9f17902db9392dcb8eb050fc44784505370806676e1672decc57738f21713469bd3039791011a309ffe11229a1cf54bd4ccdb64f1e738fca6b04956709055c72a8706aa88b44318bbc67b01a86817f42f94f645f2e395c03d7abb8dc12d985073c1bb548e046353f87c7991d9b140e9").ToPositiveBigInteger();
        public static readonly BigInteger Root2Mult2Pow7680Minus1 = new BitString("b504f333f9de6484597d89b3754abe9f1d6f60ba893ba84ced17ac85833399154afc83043ab8a2c3a8b1fe6fdc83db390f74a85e439c7b4a780487363dfa2768d2202e8742af1f4e53059c6011bc337bcab1bc911688458a460abc722f7c4e33c6d5a8a38bb7e9dccb2a634331f3c84df52f120f836e582eeaa4a0899040ca4a81394ab6d8fd0efdf4d3a02cebc93e0c4264dabcd528b651b8cf341b6f8236c70104dc01fe32352f332a5e9f7bda1ebff6a1be3fca221307dea06241f7aa81c2c1fcbddea2f7dc3318838a2eaff5f3b2d24f4a763facb882fdfe170fd3b1f780f9acce41797f2805c246785e929570235fcf8f7bca3ea33b4d7c60a5e633e3e1485f3b494d82bc6085ac27da43e4927adb8fc16e69481b04ef744894c1ea75568775190fba44fa353f48185f107dbb4a77dac64cc266eb850ed4822e1e899d034211eb71c181ec80dd4ed1a3b3423cb62e6acb96e07f9aa061a094a16b203080f7b7e36f488a515a79246344e3005da0545ab5820feaef3706e86336a418ff3fffababf23884c066deae134242ed2f48d9f17902db9392dcb8eb050fc44784505370806676e1672decc57738f21713469bd3039791011a309ffe11229a1cf54bd4ccdb64f1e738fca6b04956709055c72a8706aa88b44318bbc67b01a86817f42f94f645f2e395c03d7abb8dc12d985073c1bb548e046353f87c7991d9b140e912b44e05ad41023edcc4fb1d45327428cde06860f111402426ca7a7cdd9ea598ea44eba918dae319e24064b5f2a4dfaecb33c5a69626ed433dec724014ff54640b9ab3e15d1e9e74eff05e8da6ebb88bc02bdb4adbf578d82e11646aff75e83bff64b6dc7bbc7e0e15dde70da4f5fad7a2304414ac56e80e53f8db5e05bf60de3705376de33fc2d93a70430d9d09bab8d8b2a4c39e908e355734ec00f2bca22de3051f0527ec4b475bca5ee3816b4a2ed4a5825220655e4a1c3e1e937e879df457b182d29d0bb94456509fda0364c148aec1dd069aac6c0ee88acf4b21f5513f5bbabd10294badb7a5444f1e8495d8e342a406e174cca3d9b96f6d02f10c97cad8dc932895a026198c0eb251acd44db7c43279eaba98c9acd51c312bd49d2bd2cbbaa3fd07b0366197754277b881e9696abdb60a9d2f9be163bff14946990d94a3875720ac59e76e3256f0f218d09c5dbdf3982da37bff05e3aa7f9f60215ee42d992ccb291543dc321f1e8bc387c612dacbc0306aadc17ea769f9f0ac495ada91b0c1659418fa26513d68d337eab7ee14d3e3d49f213cd5096a30506a4a4adb5f26e39eefcb774054efe116c69c2f51766400eba060cc863df9422c240293dd").ToPositiveBigInteger();

        private static readonly BigInteger _2Pow512MinusFloorRoot2Mult2Pow1024Minus1 = new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D898").ToPositiveBigInteger();
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
                // TODO vvv confirmation vvv
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 6144, true, 44),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 6144, false, 4),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 8192, true, 44),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 8192, false, 4),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 15360, true, 44),
                (PrimeTestModes.TwoPowSecurityStrengthErrorBound, 15360, false, 4),
                // TODO ^^^ confirmation ^^^

                (PrimeTestModes.TwoPow100ErrorBound, 1024, true, 38),
                (PrimeTestModes.TwoPow100ErrorBound, 1024, false, 7),
                (PrimeTestModes.TwoPow100ErrorBound, 1536, false, 4),    // 1536 only used with 2^100 bound, with no aux primes
                (PrimeTestModes.TwoPow100ErrorBound, 2048, true, 32),
                (PrimeTestModes.TwoPow100ErrorBound, 2048, false, 4),
                (PrimeTestModes.TwoPow100ErrorBound, 3072, true, 27),
                (PrimeTestModes.TwoPow100ErrorBound, 3072, false, 3),
                (PrimeTestModes.TwoPow100ErrorBound, 4096, true, 22),
                (PrimeTestModes.TwoPow100ErrorBound, 4096, false, 2),
                // TODO vvv confirmation vvv
                (PrimeTestModes.TwoPow100ErrorBound, 6144, true, 22),
                (PrimeTestModes.TwoPow100ErrorBound, 6144, false, 2),
                (PrimeTestModes.TwoPow100ErrorBound, 8192, true, 22),
                (PrimeTestModes.TwoPow100ErrorBound, 8192, false, 2),
                (PrimeTestModes.TwoPow100ErrorBound, 15360, true, 22),
                (PrimeTestModes.TwoPow100ErrorBound, 15360, false, 2),
                // TODO ^^^ confirmation ^^^
            };

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
                case 6144:
                    lowerBound = Root2Mult2Pow3072Minus1;
                    break;
                case 8192:
                    lowerBound = Root2Mult2Pow4096Minus1;
                    break;
                case 15360:
                    lowerBound = Root2Mult2Pow7680Minus1;
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

            BigInteger p = 0, p0 = 0, p1 = 0, p2 = 0;
            BigInteger pSeed = 0, p0Seed = 0, p2Seed = 0;

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
            BigInteger lowerBound = 0;
            BigInteger modulo = 0;

            switch (L)
            {
                case 1024 / 2:
                    lowerBound = Root2Mult2Pow512Minus1;
                    modulo = _2Pow512MinusFloorRoot2Mult2Pow1024Minus1;
                    break;
                case 2048 / 2:
                    lowerBound = Root2Mult2Pow1024Minus1;
                    modulo = _2Pow1024MinusFloorRoot2Mult2Pow1024Minus1;
                    break;
                case 3072 / 2:
                    lowerBound = Root2Mult2Pow1536Minus1;
                    modulo = _2Pow1536MinusFloorRoot2Mult2Pow1536Minus1;
                    break;
                case 4096 / 2:
                    lowerBound = Root2Mult2Pow2048Minus1;
                    modulo = NumberTheory.Pow2(2048) - Root2Mult2Pow2048Minus1; // TODO need to confirm value for 2048
                    break;
                case 6144 / 2:
                    lowerBound = Root2Mult2Pow3072Minus1;
                    modulo = NumberTheory.Pow2(3072) - Root2Mult2Pow3072Minus1;
                    break;
                case 8192 / 2:
                    lowerBound = Root2Mult2Pow4096Minus1;
                    modulo = NumberTheory.Pow2(4096) - Root2Mult2Pow4096Minus1;
                    break;
                case 15360 / 2:
                    lowerBound = Root2Mult2Pow7680Minus1;
                    modulo = NumberTheory.Pow2(7680) - Root2Mult2Pow7680Minus1;
                    break;
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
