using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class ProvableProbablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        private int _bitlen1, _bitlen2, _bitlen3, _bitlen4;

        public ProvableProbablePrimesWithConditionsGenerator(HashFunction hashFunction) : base(hashFunction) { }

        public void SetBitlens(int bitlen1, int bitlen2, int bitlen3, int bitlen4)
        {
            _bitlen1 = bitlen1;
            _bitlen2 = bitlen2;
            _bitlen3 = bitlen3;
            _bitlen4 = bitlen4;
        }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq;
            BigInteger primeSeed;

            if (_bitlen1 == 0 || _bitlen2 == 0 || _bitlen3 == 0 || _bitlen4 == 0)
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
            var p1Result = ShaweTaylorRandomPrime(_bitlen1, seed.ToPositiveBigInteger());
            if (!p1Result.Success)
            {
                return new PrimeGeneratorResult("Failed to generate p1");
            }

            p1 = p1Result.Prime;
            primeSeed = p1Result.PrimeSeed;

            var p2Result = ShaweTaylorRandomPrime(_bitlen2, primeSeed);
            if (!p2Result.Success)
            {
                return new PrimeGeneratorResult("Failed to generate p2");
            }

            p2 = p2Result.Prime;
            primeSeed = p2Result.PrimeSeed;

            var pResult = ProbablePrimeFactor(p1, p2, nlen, e, security_strength);
            if (!pResult.Status)
            {
                return new PrimeGeneratorResult("Failed to generate p");
            }

            p = pResult.P;
            xp = pResult.XP;

            do
            {
                // 6
                var q1Result = ShaweTaylorRandomPrime(_bitlen1, primeSeed);
                if (!q1Result.Success)
                {
                    return new PrimeGeneratorResult("Failed to generate q1");
                }

                q1 = q1Result.Prime;
                primeSeed = q1Result.PrimeSeed;

                var q2Result = ShaweTaylorRandomPrime(_bitlen2, primeSeed);
                if (!q2Result.Success)
                {
                    return new PrimeGeneratorResult("Failed to generate q2");
                }

                q2 = q2Result.Prime;
                primeSeed = q2Result.PrimeSeed;

                var qResult = ProbablePrimeFactor(q1, q2, nlen, e, security_strength);
                if (!qResult.Status)
                {
                    return new PrimeGeneratorResult("Failed to generate q");
                }

                q = qResult.P;
                xq = qResult.XP;

            // 7
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100) ||
                     BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(nlen / 2 - 100));
            
            return new PrimeGeneratorResult(p, q);
        }
    }
}
