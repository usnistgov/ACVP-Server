using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using PrimeGeneratorResult = NIST.CVP.Crypto.Common.Asymmetric.RSA2.PrimeGenerators.PrimeGeneratorResult;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class ProvableProbablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        public ProvableProbablePrimesWithConditionsGenerator(ISha sha, IEntropyProvider entropyProvider)
            : base(sha, entropyProvider) { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq;

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

                default:
                    return new PrimeGeneratorResult("Incorrect nlen, must be 1024, 2048, 3072");
            }

            // 4
            if (seed.BitLength != 2 * securityStrength)
            {
                return new PrimeGeneratorResult("Incorrect seed length");
            }

            // 5
            var p1Result = PrimeGen186_4.ShaweTaylorRandomPrime(_bitlens[0], seed.ToPositiveBigInteger(), _sha);
            if (!p1Result.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p1: {p1Result.ErrorMessage}");
            }

            var p2Result = PrimeGen186_4.ShaweTaylorRandomPrime(_bitlens[1], p1Result.PrimeSeed, _sha);
            if (!p2Result.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p2: {p2Result.ErrorMessage}");
            }

            p1 = p1Result.Prime;
            p2 = p2Result.Prime;

            var pResult = ProbablePrimeFactor(p1, p2, nlen, e, securityStrength);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p: {pResult.ErrorMessage}");
            }

            p = pResult.Prime;
            xp = pResult.XPrime;

            PrimeGen186_4.STRandomPrimeResult q1Result;
            do
            {
                // 6
                q1Result = PrimeGen186_4.ShaweTaylorRandomPrime(_bitlens[2], p2Result.PrimeSeed, _sha);
                if (!q1Result.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q1: {q1Result.ErrorMessage}");
                }

                var q2Result = PrimeGen186_4.ShaweTaylorRandomPrime(_bitlens[3], q1Result.PrimeSeed, _sha);
                if (!q2Result.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q2: {q2Result.ErrorMessage}");
                }

                q1 = q1Result.Prime;
                q2 = q2Result.Prime;

                var qResult = ProbablePrimeFactor(q1, q2, nlen, e, securityStrength);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q: {qResult.ErrorMessage}");
                }

                q = qResult.Prime;
                xq = qResult.XPrime;

            // 7
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100) ||
                     BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(nlen / 2 - 100));

            var auxValues = new AuxiliaryResult { XP = xp, XQ = xq };
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
