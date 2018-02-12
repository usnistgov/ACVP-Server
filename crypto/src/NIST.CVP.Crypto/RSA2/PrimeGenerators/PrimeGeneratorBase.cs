using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public abstract class PrimeGeneratorBase : IPrimeGenerator
    {
        private readonly BigInteger _root2Mult2Pow512Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768").ToPositiveBigInteger();
        protected readonly BigInteger _root2Mult2Pow1024Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A").ToPositiveBigInteger();
        protected readonly BigInteger _root2Mult2Pow1536Minus1 = new BitString("B504F333F9DE6484597D89B3754ABE9F1D6F60BA893BA84CED17AC85833399154AFC83043AB8A2C3A8B1FE6FDC83DB390F74A85E439C7B4A780487363DFA2768D2202E8742AF1F4E53059C6011BC337BCAB1BC911688458A460ABC722F7C4E33C6D5A8A38BB7E9DCCB2A634331F3C84DF52F120F836E582EEAA4A0899040CA4A81394AB6D8FD0EFDF4D3A02CEBC93E0C4264DABCD528B651B8CF341B6F8236C70104DC01FE32352F332A5E9F7BDA1EBFF6A1BE3FCA221307DEA06241F7AA81C2").ToPositiveBigInteger();
        private readonly BigInteger _2Pow512MinusFloorRoot2Mult2Pow1024Minus1 = new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D898").ToPositiveBigInteger();
        private readonly BigInteger _2Pow1024MinusFloorRoot2Mult2Pow1024Minus1 = new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D8972DDFD178BD50E0B1ACFA639FEE43CC84354E436EE977BA75B9F5438DD083B1CC392A575C7448162334D59CBCCE0C37B20AD0EDF07C91A7D1155B5F766FBF35B6").ToPositiveBigInteger();
        private readonly BigInteger _2Pow1536MinusFloorRoot2Mult2Pow1536Minus1 = new BitString("4AFB0CCC06219B7BA682764C8AB54160E2909F4576C457B312E8537A7CCC66EAB5037CFBC5475D3C574E0190237C24C6F08B57A1BC6384B587FB78C9C205D8972DDFD178BD50E0B1ACFA639FEE43CC84354E436EE977BA75B9F5438DD083B1CC392A575C7448162334D59CBCCE0C37B20AD0EDF07C91A7D1155B5F766FBF35B57EC6B5492702F1020B2C5FD31436C1F3BD9B25432AD749AE4730CBE4907DC938FEFB23FE01CDCAD0CCD5A1608425E140095E41C035DDECF8215F9DBE08557E3E").ToPositiveBigInteger();

        protected readonly ISha _sha;
        protected IEntropyProvider _entropyProvider;
        private readonly PrimeTestModes _primeTestMode;
        protected int[] _bitlens = {0, 0, 0, 0};

        protected PrimeGeneratorBase(ISha sha = null, IEntropyProvider entropyProvider = null, PrimeTestModes primeTestMode = PrimeTestModes.C2)
        {
            _sha = sha;
            _entropyProvider = entropyProvider;
            _primeTestMode = primeTestMode;
        }

        public void SetBitlens(int b1, int b2, int b3, int b4)
        {
            _bitlens = new[] { b1, b2, b3, b4 };
        }

        public void SetBitlens(int[] bitlens)
        {
            _bitlens = bitlens;
        }

        /// <summary>
        /// C.9 Compute a Probable Prime Factor Based on Auxiliary Primes
        /// </summary>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="nlen"></param>
        /// <param name="e"></param>
        /// <param name="securityStrength"></param>
        /// <returns></returns>
        public PpfResult ProbablePrimeFactor(BigInteger r1, BigInteger r2, int nlen, BigInteger e, int securityStrength)
        {
            var i = 0;
            BigInteger X, Y;
            BigInteger lowerBound, upperBound;
            upperBound = NumberTheory.Pow2(nlen / 2) - 1;
            if (nlen == 1024)
            {
                lowerBound = _root2Mult2Pow512Minus1;
            }
            else if (nlen == 2048)
            {
                lowerBound = _root2Mult2Pow1024Minus1;
            }
            else if (nlen == 3072)
            {
                lowerBound = _root2Mult2Pow1536Minus1;
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

                do
                {
                    // 3
                    X = _entropyProvider.GetEntropy(lowerBound, upperBound);

                    // 4
                    // Weirdness to ensure Y is positive
                    Y = X + (((R - X) % modulo) + modulo) % modulo;

                    // 5
                    i = 0;

                    // 6
                } while (Y >= NumberTheory.Pow2(nlen / 2));

                do
                {
                    // 7
                    if (NumberTheory.GCD(Y - 1, e) == 1)
                    {
                        if (MillerRabin(nlen, Y, true))
                        {
                            return new PpfResult(Y, X);
                        }
                    }

                    // 8
                    i++;

                    // 9
                    if (i >= 5 * (nlen / 2))
                    {
                        return new PpfResult("PPF: Too many iterations");
                    }

                    // 10
                    Y += modulo;

                    // 6 (repeated)
                } while (Y < NumberTheory.Pow2(nlen / 2));
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
        public PpcResult ProvablePrimeConstruction(int L, int N1, int N2, BigInteger firstSeed, BigInteger e)
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
                var stResult = PrimeGen186_4.ShaweTaylorRandomPrime(N1, firstSeed, _sha);
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
                var stResult = PrimeGen186_4.ShaweTaylorRandomPrime(N2, p2Seed, _sha);
                if (!stResult.Success)
                {
                    return new PpcResult("PPC: fail ST p2 gen");
                }

                p2 = stResult.Prime;
                p0Seed = stResult.PrimeSeed;
            }

            // 6
            var result = PrimeGen186_4.ShaweTaylorRandomPrime((int)System.Math.Ceiling(L / 2.0) + 1, p0Seed, _sha);
            if (!result.Success)
            {
                return new PpcResult("PPC: fail ST p0 gen");
            }

            p0 = result.Prime;
            pSeed = result.PrimeSeed;

            // 7, 8, 9
            var outLen = _sha.HashFunction.OutputLen;
            var iterations = L.CeilingDivide(outLen) - 1;
            var pGenCounter = 0;
            BigInteger x = 0;

            // 10
            for (var i = 0; i <= iterations; i++)
            {
                x += _sha.HashNumber(pSeed + i).ToBigInteger() * NumberTheory.Pow2(i * outLen);
            }

            // 11
            pSeed += iterations + 1;

            // 12
            // sqrt(2) * 2^(L-1)
            BigInteger lowerBound;
            BigInteger modulo;

            if (L == 1024 / 2)
            {
                lowerBound = _root2Mult2Pow512Minus1;
                modulo = _2Pow512MinusFloorRoot2Mult2Pow1024Minus1;
            }
            else if (L == 2048 / 2)
            {
                lowerBound = _root2Mult2Pow1024Minus1;
                modulo = _2Pow1024MinusFloorRoot2Mult2Pow1024Minus1;
            }
            else if (L == 3072 / 2)
            {
                lowerBound = _root2Mult2Pow1536Minus1;
                modulo = _2Pow1536MinusFloorRoot2Mult2Pow1536Minus1;
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
                        a += _sha.HashNumber(pSeed + i).ToBigInteger() * NumberTheory.Pow2(i * outLen);
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
        protected bool MillerRabin(int nlen, BigInteger val, bool factor)
        {
            if (nlen == 2048)
            {
                if (_primeTestMode == PrimeTestModes.C2)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 38);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 5);
                    }
                }
                else // if (_primeTestMode == PrimeTestModes.C3)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 41);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 4);
                    }
                }
            }
            else // if(nlen == 3072)
            {
                if (_primeTestMode == PrimeTestModes.C2)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 32);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 4);
                    }
                }
                else // if (_primeTestMode == PrimeTestModes.C3)
                {
                    if (factor)
                    {
                        return NumberTheory.MillerRabin(val, 27);
                    }
                    else
                    {
                        return NumberTheory.MillerRabin(val, 3);
                    }
                }
            }
        }

        public abstract PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed);
    }
}
