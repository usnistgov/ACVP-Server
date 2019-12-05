using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math.Entropy;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class ProvableProbablePrimesWithConditionsGenerator : IFips186_4PrimeGenerator, IFips186_5PrimeGenerator
    {
        private readonly ISha _sha;
        private readonly IEntropyProvider _entropyProvider;
        private readonly PrimeTestModes _primeTest;

        private int _pBound = 5;
        
        public ProvableProbablePrimesWithConditionsGenerator(ISha sha, IEntropyProvider entropyProvider, PrimeTestModes primeTest)
        {
            _sha = sha;
            _entropyProvider = entropyProvider;
            _primeTest = primeTest;
        }

        public PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param)
        {
            // Ensure these are not used
            param.A = 0;
            param.B = 0;
            
            var errors = new List<string>();
            
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_4(param.Modulus, errors);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE, errors);
            PrimeGeneratorGuard.AgainstInvalidSeed(param.Modulus, param.Seed, errors);
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
            PrimeGeneratorGuard.AgainstInvalidSeed(param.Modulus, param.Seed, errors);
            PrimeGeneratorGuard.AgainstInvalidBitlens(param.Modulus, param.BitLens, errors);

            if (errors.Any())
            {
                return new PrimeGeneratorResult(string.Join(".", errors));
            }
            
            return GeneratePrimes(param);
        }

        private PrimeGeneratorResult GeneratePrimes(PrimeGeneratorParameters param)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq;

            // 1, 2, 3, 4 covered by guards

            // 5
            var p1Result = PrimeGen186_4.ShaweTaylorRandomPrime(param.BitLens[0], param.Seed.ToPositiveBigInteger(), _sha);
            if (!p1Result.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p1: {p1Result.ErrorMessage}");
            }

            var p2Result = PrimeGen186_4.ShaweTaylorRandomPrime(param.BitLens[1], p1Result.PrimeSeed, _sha);
            if (!p2Result.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p2: {p2Result.ErrorMessage}");
            }

            p1 = p1Result.Prime;
            p2 = p2Result.Prime;

            var pResult = PrimeGeneratorHelper.ProbablePrimeFactor(_primeTest, _entropyProvider, _pBound, param.A, p1, p2, param.Modulus, param.PublicE);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p: {pResult.ErrorMessage}");
            }

            p = pResult.Prime;
            xp = pResult.XPrime;

            do
            {
                // 6
                var q1Result = PrimeGen186_4.ShaweTaylorRandomPrime(param.BitLens[2], p2Result.PrimeSeed, _sha);
                if (!q1Result.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q1: {q1Result.ErrorMessage}");
                }

                var q2Result = PrimeGen186_4.ShaweTaylorRandomPrime(param.BitLens[3], q1Result.PrimeSeed, _sha);
                if (!q2Result.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q2: {q2Result.ErrorMessage}");
                }

                q1 = q1Result.Prime;
                q2 = q2Result.Prime;

                var qResult = PrimeGeneratorHelper.ProbablePrimeFactor(_primeTest, _entropyProvider, _pBound, param.B, q1, q2, param.Modulus, param.PublicE);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q: {qResult.ErrorMessage}");
                }

                q = qResult.Prime;
                xq = qResult.XPrime;

            // 7
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(param.Modulus / 2 - 100) ||
                     BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(param.Modulus / 2 - 100));

            var auxValues = new AuxiliaryResult { XP = xp, XQ = xq };
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
