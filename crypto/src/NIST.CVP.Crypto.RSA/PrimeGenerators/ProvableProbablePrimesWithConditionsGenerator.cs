using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    // B.3.5
    public class ProvableProbablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        public ProvableProbablePrimesWithConditionsGenerator(HashFunction hashFunction, EntropyProviderTypes type) : base(hashFunction, type) { }

        public ProvableProbablePrimesWithConditionsGenerator() { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq;

            if (_bitlens[0] == 0 || _bitlens[1] == 0 || _bitlens[2] == 0 || _bitlens[3] == 0)
            {
                return new PrimeGeneratorResult("Empty bitlens, must be assigned outside of GeneratePrimes()");
            }

            // 1
            if (nlen != 1024 && nlen != 2048 && nlen != 3072)
            {
                return new PrimeGeneratorResult("Incorrect nlen, must be 1024, 2048, 3072");
            }

            // 2
            if (e <= NumberTheory.Pow2(16) || e >= NumberTheory.Pow2(256) || e.IsEven)
            {
                return new PrimeGeneratorResult("Incorrect e, must be greater than 2^16, less than 2^256, odd");
            }

            // 3
            int security_strength;
            if (nlen == 1024)
            {
                security_strength = 80;
            }
            else if (nlen == 2048)
            {
                security_strength = 112;
            }
            else // if (nlen == 3072)
            {
                security_strength = 128;
            }

            // 4
            if (seed.BitLength != 2 * security_strength)
            {
                return new PrimeGeneratorResult("Incorrect seed length");
            }

            // 5
            var p1Result = ShaweTaylorRandomPrime(_bitlens[0], seed.ToPositiveBigInteger());
            if (!p1Result.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p1: {p1Result.ErrorMessage}");
            }

            var p2Result = ShaweTaylorRandomPrime(_bitlens[1], p1Result.PrimeSeed);
            if (!p2Result.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p2: {p2Result.ErrorMessage}");
            }

            p1 = p1Result.Prime;
            p2 = p2Result.Prime;

            var pResult = ProbablePrimeFactor(p1, p2, nlen, e, security_strength);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p: {pResult.ErrorMessage}");
            }

            p = pResult.P;
            xp = pResult.XP;

            STRandomPrimeResult q1Result;
            do
            {
                // 6
                q1Result = ShaweTaylorRandomPrime(_bitlens[2], p2Result.PrimeSeed);
                if (!q1Result.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q1: {q1Result.ErrorMessage}");
                }

                var q2Result = ShaweTaylorRandomPrime(_bitlens[3], q1Result.PrimeSeed);
                if (!q2Result.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q2: {q2Result.ErrorMessage}");
                }

                q1 = q1Result.Prime;
                q2 = q2Result.Prime;

                var qResult = ProbablePrimeFactor(q1, q2, nlen, e, security_strength);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q: {qResult.ErrorMessage}");
                }

                q = qResult.P;
                xq = qResult.XP;

            // 7
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100) ||
                     BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(nlen / 2 - 100));

            var auxValues = new AuxiliaryPrimeGeneratorResult(xp, xq);
            return new PrimeGeneratorResult(p, q, auxValues);
        }
    }
}
