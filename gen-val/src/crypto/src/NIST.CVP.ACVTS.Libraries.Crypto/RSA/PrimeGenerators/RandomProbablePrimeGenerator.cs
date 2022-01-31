using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.RSA.PrimeGenerators
{
    public class RandomProbablePrimeGenerator : IFips186_2PrimeGenerator, IFips186_4PrimeGenerator, IFips186_5PrimeGenerator
    {
        private readonly IEntropyProvider _entropyProvider;
        private readonly PrimeTestModes _primeTestMode;

        // Default properties for PrimeGenerator
        private bool _kat = false;
        private bool _performAShift = false;
        private bool _performBShift = false;
        private int _iBoundForP = 5;
        private int _iBoundForQ = 5;

        public RandomProbablePrimeGenerator(IEntropyProvider entropyProvider, PrimeTestModes primeTestMode)
        {
            _entropyProvider = entropyProvider;
            _primeTestMode = primeTestMode;
        }

        public PrimeGeneratorResult GeneratePrimesKat(PrimeGeneratorParameters param)
        {
            _kat = true;
            return GeneratePrimes(param);
        }

        public PrimeGeneratorResult GeneratePrimesFips186_2(PrimeGeneratorParameters param)
        {
            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_2(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponentFips186_2(param.PublicE);

            return GeneratePrimes(param);
        }

        public PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param)
        {
            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_4(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE);

            return GeneratePrimes(param);
        }

        public PrimeGeneratorResult GeneratePrimesFips186_5(PrimeGeneratorParameters param)
        {
            _iBoundForP = 10;
            _iBoundForQ = 20;

            _performAShift = param.A != default(int);
            _performBShift = param.B != default(int);

            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_5(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE);
            PrimeGeneratorGuard.AgainstInvalidAB(param.A);
            PrimeGeneratorGuard.AgainstInvalidAB(param.B);

            return GeneratePrimes(param);
        }

        private PrimeGeneratorResult GeneratePrimes(PrimeGeneratorParameters param)
        {
            // 1, 2, 3 performed by guards

            // 4, 4.1
            var i = 0;
            BigInteger p = 0;
            var pqLowerBound = GetBound(param.Modulus);

            do
            {
                do
                {
                    // 4.2
                    if (p != 0 && _kat)
                    {
                        return new PrimeGeneratorResult("Given p less than sqrt(2) * 2 ^ (n/2) - 1, need to get a new random number.");
                    }
                    p = _entropyProvider.GetEntropy(param.Modulus / 2).ToPositiveBigInteger();

                    // 4.3
                    if (_performAShift)
                    {
                        p += (param.A - p).PosMod(8);
                    }
                    else if (p.IsEven)
                    {
                        p++;
                    }

                    // 4.4
                } while (p < pqLowerBound);

                // 4.5
                if (NumberTheory.GCD(p - 1, param.PublicE) == 1)
                {
                    if (PrimeGeneratorHelper.MillerRabin(_primeTestMode, param.Modulus, p, false))
                    {
                        break;
                    }
                }

                // 4.6, 4.7
                i++;
                if (i >= _iBoundForP * (param.Modulus / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for p");
                }

                if (_kat)
                {
                    return new PrimeGeneratorResult("Given p is not prime");
                }
            } while (!_kat);

            // 5, 5.1
            i = 0;
            BigInteger q = 0;
            do
            {
                do
                {
                    // 5.2
                    if (q != 0 && _kat)
                    {
                        return new PrimeGeneratorResult("Given q less than sqrt(2) * 2 ^ (n/2) - 1, need to get a new random number.");
                    }
                    q = _entropyProvider.GetEntropy(param.Modulus / 2).ToPositiveBigInteger();

                    // 5.3
                    if (_performBShift)
                    {
                        q += (param.B - q).PosMod(8);
                    }
                    else if (q.IsEven)
                    {
                        q++;
                    }

                    // 5.4
                    // 5.5
                } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(param.Modulus / 2 - 100) || q < pqLowerBound);

                // 5.6
                if (NumberTheory.GCD(q - 1, param.PublicE) == 1)
                {
                    if (PrimeGeneratorHelper.MillerRabin(_primeTestMode, param.Modulus, q, false))
                    {
                        break;
                    }
                }

                // 5.7, 5.8
                i++;
                if (i >= _iBoundForQ * (param.Modulus / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for q");
                }

                if (_kat)
                {
                    return new PrimeGeneratorResult("Given q is not prime");
                }
            } while (!_kat);

            var auxValues = new AuxiliaryResult();
            var primePair = new PrimePair { P = p, Q = q };
            return new PrimeGeneratorResult(primePair, auxValues);
        }

        private BigInteger GetBound(int nlen)
        {
            switch (nlen)
            {
                case 1024:
                    return PrimeGeneratorHelper.Root2Mult2Pow512Minus1;
                case 1536:
                    return PrimeGeneratorHelper.Root2Mult2Pow768Minus1;
                case 2048:
                    return PrimeGeneratorHelper.Root2Mult2Pow1024Minus1;
                case 3072:
                    return PrimeGeneratorHelper.Root2Mult2Pow1536Minus1;
                case 4096:
                    return PrimeGeneratorHelper.Root2Mult2Pow2048Minus1;
                case 6144:
                    return PrimeGeneratorHelper.Root2Mult2Pow3072Minus1;
                case 8192:
                    return PrimeGeneratorHelper.Root2Mult2Pow4096Minus1;
                case 15360:
                    return PrimeGeneratorHelper.Root2Mult2Pow7680Minus1;
                default:
                    throw new ArgumentException("Invalid nlen");
            }
        }
    }
}
