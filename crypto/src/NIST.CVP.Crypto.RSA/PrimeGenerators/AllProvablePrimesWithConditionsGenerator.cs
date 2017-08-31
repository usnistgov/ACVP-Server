using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;
using NIST.CVP.Crypto.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    // B.3.4
    public class AllProvablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        public AllProvablePrimesWithConditionsGenerator(HashFunction hashFunction) : base(hashFunction) { }
        public AllProvablePrimesWithConditionsGenerator() { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2;

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
            var workingSeed = seed.ToPositiveBigInteger();

            // 6
            var pResult = ProvablePrimeConstruction(nlen / 2, _bitlens[0], _bitlens[1], workingSeed, e);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Bad p gen: {pResult.ErrorMessage}");
            }

            p = pResult.P;
            p1 = pResult.P1;
            p2 = pResult.P2;
            workingSeed = pResult.PSeed;

            do
            {
                // 7
                var qResult = ProvablePrimeConstruction(nlen / 2, _bitlens[2], _bitlens[3], workingSeed, e);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Bad q gen: {qResult.ErrorMessage}");
                }

                q = qResult.P;
                q1 = qResult.P1;
                q2 = qResult.P2;
                workingSeed = qResult.PSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100));
            
            return new PrimeGeneratorResult(p, q);
        }
    }
}
