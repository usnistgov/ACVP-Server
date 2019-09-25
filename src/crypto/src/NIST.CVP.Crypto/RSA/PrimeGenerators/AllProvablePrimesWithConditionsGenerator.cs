using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class AllProvablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        public AllProvablePrimesWithConditionsGenerator(ISha sha) : base(sha) { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2;

            if (_bitlens.Length != 4)
            {
                return new PrimeGeneratorResult("Needs exactly 4 bitlen values");
            }

            if (_bitlens[0] == 0 || _bitlens[1] == 0 || _bitlens[2] == 0 || _bitlens[3] == 0)
            {
                return new PrimeGeneratorResult("Empty bitlens, must be assigned outside of GeneratePrimes()");
            }

            // 1 -- redundant by (3)
            // 2
            if (e <= NumberTheory.Pow2(16) || e >= NumberTheory.Pow2(256) || e.IsEven)
            {
                return new PrimeGeneratorResult("Incorrect e, must be greater than 2^16, less than 2^256, odd");
            }

            // 3
            int securityStrength;
            switch (nlen)
            {
                case 1024:
                    securityStrength = 80;
                    break;

                case 2048:
                    securityStrength = 112;
                    break;

                case 3072:
                    securityStrength = 128;
                    break;
                
                case 4096:
                    securityStrength = 128;
                    break;

                default:
                    return new PrimeGeneratorResult("Incorrect nlen, must be 1024, 2048, 3072, 4096");
            }

            // 4
            if (seed.BitLength != 2 * securityStrength)
            {
                return new PrimeGeneratorResult("Incorrect seed length");
            }

            // 5
            var workingSeed = seed.ToPositiveBigInteger();

            // 6
            var pResult = ProvablePrimeConstruction(nlen / 2, _bitlens[0], _bitlens[1], workingSeed, e);
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
                var qResult = ProvablePrimeConstruction(nlen / 2, _bitlens[2], _bitlens[3], workingSeed, e);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Bad q gen: {qResult.ErrorMessage}");
                }

                q = qResult.Prime;
                q1 = qResult.Prime1;
                q2 = qResult.Prime2;
                workingSeed = qResult.PrimeSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100));

            var auxValues = new AuxiliaryResult();
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
