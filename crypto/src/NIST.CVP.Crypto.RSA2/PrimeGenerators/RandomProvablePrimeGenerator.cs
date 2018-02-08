using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class RandomProvablePrimeGenerator : PrimeGeneratorBase
    {
        public RandomProvablePrimeGenerator(ISha sha) : base(sha) { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            // 1 -- redundant by (3)
            // 2
            if (e <=  NumberTheory.Pow2(16) || e >= NumberTheory.Pow2(256) || e.IsEven)
            {
                return new PrimeGeneratorResult("Incorrect e, must be greater than 2^16, less than 2^256, odd");
            }

            // 3
            int securityStrength;
            switch (nlen)
            {
                case 2048:
                    securityStrength = 112;
                    break;

                case 3072:
                    securityStrength = 128;
                    break;

                default:
                    return new PrimeGeneratorResult("Incorrect nlen, must be 2048, 3072");
            }

            // 4
            if (seed.BitLength != 2 * securityStrength)
            {
                return new PrimeGeneratorResult("Incorrect seed length");
            }

            // 5
            var workingSeed = seed.ToPositiveBigInteger();

            // 6
            var ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
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
                ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
                if (!ppcResult.Success)
                {
                    return new PrimeGeneratorResult($"Bad Provable Prime Construction for q: {ppcResult.ErrorMessage}");
                }
                q = ppcResult.Prime;
                workingSeed = ppcResult.PrimeSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100));

            // 9, 10
            var auxValues = new AuxiliaryResult();
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
