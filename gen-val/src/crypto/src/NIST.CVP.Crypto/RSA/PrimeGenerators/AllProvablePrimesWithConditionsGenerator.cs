using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class AllProvablePrimesWithConditionsGenerator : IFips186_4PrimeGenerator, IFips186_5PrimeGenerator
    {
        private readonly ISha _sha;

        public AllProvablePrimesWithConditionsGenerator(ISha sha)
        {
            _sha = sha;
        }

        public PrimeGeneratorResult GeneratePrimesFips186_4(PrimeGeneratorParameters param)
        {
            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_4(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE);
            PrimeGeneratorGuard.AgainstInvalidSeed(param.Modulus, param.Seed);
            PrimeGeneratorGuard.AgainstInvalidBitlens(param.Modulus, param.BitLens);

            return GeneratePrimes(param);
        }

        public PrimeGeneratorResult GeneratePrimesFips186_5(PrimeGeneratorParameters param)
        {
            // Rethrow on exception
            PrimeGeneratorGuard.AgainstInvalidModulusFips186_5(param.Modulus);
            PrimeGeneratorGuard.AgainstInvalidPublicExponent(param.PublicE);
            PrimeGeneratorGuard.AgainstInvalidSeed(param.Modulus, param.Seed);
            PrimeGeneratorGuard.AgainstInvalidBitlens(param.Modulus, param.BitLens);

            return GeneratePrimes(param);
        }

        private PrimeGeneratorResult GeneratePrimes(PrimeGeneratorParameters param)
        {
            BigInteger p, p1, p2, q, q1, q2;

            // 1, 2, 3, 4 covered by Guards

            // 5
            var workingSeed = param.Seed.ToPositiveBigInteger();

            // 6
            var pResult = PrimeGeneratorHelper.ProvablePrimeConstruction(_sha, param.Modulus / 2, param.BitLens[0], param.BitLens[1], workingSeed, param.PublicE);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Bad p gen: {pResult.ErrorMessage}");
            }

            p = pResult.Prime;
            p1 = pResult.Prime1;
            p2 = pResult.Prime2;
            workingSeed = pResult.PrimeSeed;

            do
            {
                // 7
                var qResult = PrimeGeneratorHelper.ProvablePrimeConstruction(_sha, param.Modulus / 2, param.BitLens[2], param.BitLens[3], workingSeed, param.PublicE);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Bad q gen: {qResult.ErrorMessage}");
                }

                q = qResult.Prime;
                q1 = qResult.Prime1;
                q2 = qResult.Prime2;
                workingSeed = qResult.PrimeSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(param.Modulus / 2 - 100));

            var auxValues = new AuxiliaryResult();
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
