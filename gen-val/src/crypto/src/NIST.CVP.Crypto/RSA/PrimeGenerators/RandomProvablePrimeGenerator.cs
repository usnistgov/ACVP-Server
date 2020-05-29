using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class RandomProvablePrimeGenerator : IFips186_4PrimeGenerator, IFips186_5PrimeGenerator
    {
        private readonly ISha _sha;

        public RandomProvablePrimeGenerator(ISha sha)
        {
            _sha = sha;
        }

        public PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param)
        {
            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_4(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE);
            PrimeGeneratorGuard.AgainstInvalidSeed(param.Modulus, param.Seed);

            return GeneratePrimes(param);
        }

        public PrimeGeneratorResult GeneratePrimesFips186_5(PrimeGeneratorParameters param)
        {
            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_5(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE);
            PrimeGeneratorGuard.AgainstInvalidSeed(param.Modulus, param.Seed);

            return GeneratePrimes(param);
        }

        private PrimeGeneratorResult GeneratePrimes(PrimeGeneratorParameters param)
        {
            // 1, 2, 3, 4 covered by guards

            // 5
            var workingSeed = param.Seed.ToPositiveBigInteger();

            // 6
            var ppcResult = PrimeGeneratorHelper.ProvablePrimeConstruction(_sha, param.Modulus / 2, 1, 1, workingSeed, param.PublicE);
            if (!ppcResult.Success)
            {
                return new PrimeGeneratorResult($"Bad Provable Prime Construction for p: {ppcResult.ErrorMessage}");
            }
            var p = ppcResult.Prime;
            workingSeed = ppcResult.PrimeSeed;

            BigInteger q;
            do
            {
                // 7
                ppcResult = PrimeGeneratorHelper.ProvablePrimeConstruction(_sha, param.Modulus / 2, 1, 1, workingSeed, param.PublicE);
                if (!ppcResult.Success)
                {
                    return new PrimeGeneratorResult($"Bad Provable Prime Construction for q: {ppcResult.ErrorMessage}");
                }
                q = ppcResult.Prime;
                workingSeed = ppcResult.PrimeSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(param.Modulus / 2 - 100));

            // 9, 10
            var auxValues = new AuxiliaryResult();
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
