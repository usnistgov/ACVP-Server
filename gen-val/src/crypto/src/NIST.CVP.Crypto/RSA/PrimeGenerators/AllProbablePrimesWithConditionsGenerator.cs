using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math.Entropy;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class AllProbablePrimesWithConditionsGenerator : IFips186_4PrimeGenerator, IFips186_5PrimeGenerator
    {
        private readonly IEntropyProvider _entropyProvider;
        private readonly PrimeTestModes _primeTestMode;

        private int _pBound = 5;
        
        public AllProbablePrimesWithConditionsGenerator(IEntropyProvider entropyProvider, PrimeTestModes primeTestMode)
        {
            _entropyProvider = entropyProvider;
            _primeTestMode = primeTestMode;
        }

        public PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param)
        {
            // Ensure these are not used
            param.A = 0;
            param.B = 0;
            
            var errors = new List<string>();
            
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_4(param.Modulus, errors);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE, errors);
            PrimeGeneratorGuard.AgainstInvalidBitlens(param.Modulus, param.BitLens, errors);

            if (errors.Any())
            {
                return new PrimeGeneratorResult(string.Join(".", errors));
            }
            
            return GeneratePrimes(param);
        }

        public PrimeGeneratorResult GeneratePrimesFips186_5(PrimeGeneratorParameters param)
        {
            _pBound = 20;
            
            var errors = new List<string>();
            
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_5(param.Modulus, errors);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE, errors);
            PrimeGeneratorGuard.AgainstInvalidBitlens(param.Modulus, param.BitLens, errors);

            if (errors.Any())
            {
                return new PrimeGeneratorResult(string.Join(".", errors));
            }
            
            return GeneratePrimes(param);        }

        private PrimeGeneratorResult GeneratePrimes(PrimeGeneratorParameters param)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq, xp1, xp2, xq1, xq2;
            
            // 1, 2, 3 covered by guards
            
            // 4
            xp1 = _entropyProvider.GetEntropy(param.BitLens[0]).ToPositiveBigInteger();
            if (xp1.IsEven)
            {
                xp1++;
            }

            xp2 = _entropyProvider.GetEntropy(param.BitLens[1]).ToPositiveBigInteger();
            if (xp2.IsEven)
            {
                xp2++;
            }

            p1 = xp1;
            while (!PrimeGeneratorHelper.MillerRabin(_primeTestMode, param.Modulus, p1, true))
            {
                p1 += 2;
            }

            p2 = xp2;
            while (!PrimeGeneratorHelper.MillerRabin(_primeTestMode, param.Modulus, p2, true))
            {
                p2 += 2;
            }

            var pResult = PrimeGeneratorHelper.ProbablePrimeFactor(_primeTestMode, _entropyProvider, _pBound, param.A, p1, p2, param.Modulus, param.PublicE);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p: {pResult.ErrorMessage}");
            }
            p = pResult.Prime;
            xp = pResult.XPrime;

            // 5
            do
            {
                xq1 = _entropyProvider.GetEntropy(param.BitLens[2]).ToPositiveBigInteger();
                if (xq1.IsEven)
                {
                    xq1++;
                }

                xq2 = _entropyProvider.GetEntropy(param.BitLens[3]).ToPositiveBigInteger();
                if (xq2.IsEven)
                {
                    xq2++;
                }

                q1 = xq1;
                while (!PrimeGeneratorHelper.MillerRabin(_primeTestMode, param.Modulus, q1, true))
                {
                    q1 += 2;
                }

                q2 = xq2;
                while (!PrimeGeneratorHelper.MillerRabin(_primeTestMode, param.Modulus, q2, true))
                {
                    q2 += 2;
                }

                var qResult = PrimeGeneratorHelper.ProbablePrimeFactor(_primeTestMode, _entropyProvider, _pBound, param.B, q1, q2, param.Modulus, param.PublicE);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q: {qResult.ErrorMessage}");
                }
                q = qResult.Prime;
                xq = qResult.XPrime;

                // 6
            } while (BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(param.Modulus / 2 - 100) ||
                     BigInteger.Abs(p - q)   <= NumberTheory.Pow2(param.Modulus / 2 - 100));

            var auxValues = new AuxiliaryResult{ XP1 = xp1, XP2 = xp2, XP = xp, XQ1 = xq1, XQ2 = xq2, XQ = xq };
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
