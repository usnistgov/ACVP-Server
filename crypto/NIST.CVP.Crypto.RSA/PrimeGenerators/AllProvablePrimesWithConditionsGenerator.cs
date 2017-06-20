using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    // B.3.4
    public class AllProvablePrimesWithConditionsGenerator : PrimeGeneratorBase, IPrimeGenerator
    {
        private int _bitlen1, _bitlen2, _bitlen3, _bitlen4;

        public AllProvablePrimesWithConditionsGenerator(HashFunction hashFunction) : base(hashFunction) { }
        public AllProvablePrimesWithConditionsGenerator() { }

        public void SetBitlens(int bitlen1, int bitlen2, int bitlen3, int bitlen4)
        {
            _bitlen1 = bitlen1;
            _bitlen2 = bitlen2;
            _bitlen3 = bitlen3;
            _bitlen4 = bitlen4;
        }

        public void SetBitlens(int[] bitlens)
        {
            _bitlen1 = bitlens[0];
            _bitlen2 = bitlens[1];
            _bitlen3 = bitlens[2];
            _bitlen4 = bitlens[3];
        }

        public virtual PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
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
            var pResult = ProvablePrimeConstruction(nlen / 2, _bitlen1, _bitlen2, workingSeed, e);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult("Bad p gen");
            }

            p = pResult.P;
            p1 = pResult.P1;
            p2 = pResult.P2;
            workingSeed = pResult.PSeed;

            do
            {
                // 7
                var qResult = ProvablePrimeConstruction(nlen / 2, _bitlen3, _bitlen4, workingSeed, e);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult("Bad q gen");
                }

                q = qResult.P;
                q1 = qResult.P1;
                q2 = qResult.P2;
                workingSeed = qResult.PSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100));
            
            return new PrimeGeneratorResult(p, q, p1, q1, p2, q2);
        }
    }
}
