using System.Numerics;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    // B.3.2
    public class RandomProvablePrimeGenerator : PrimeGeneratorBase, IPrimeGenerator
    {
        public RandomProvablePrimeGenerator(HashFunction hashFunction) : base(hashFunction) { }
        public RandomProvablePrimeGenerator() : base() { }

        public virtual PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            // 1
            if (nlen != 2048 && nlen != 3072)
            {
                return new PrimeGeneratorResult("Incorrect nlen, must be 2048, 3072");
            }

            // 2
            if (e <=  NumberTheory.Pow2(16) || e >= NumberTheory.Pow2(256) || e.IsEven)
            {
                return new PrimeGeneratorResult("Incorrect e, must be greater than 2^16, less than 2^256, odd");
            }

            // 3
            int security_strength;
            if (nlen == 2048)
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
            var ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
            if (!ppcResult.Success)
            {
                return new PrimeGeneratorResult($"Bad Provable Prime Construction for p: {ppcResult.ErrorMessage}");
            }
            var p = ppcResult.P;
            workingSeed = ppcResult.PSeed;

            BigInteger q = 0;
            do
            {
                // 7
                ppcResult = ProvablePrimeConstruction(nlen / 2, 1, 1, workingSeed, e);
                if (!ppcResult.Success)
                {
                    return new PrimeGeneratorResult($"Bad Provable Prime Construction for q: {ppcResult.ErrorMessage}");
                }
                q = ppcResult.P;
                workingSeed = ppcResult.PSeed;

            // 8
            } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100));

            // 9, 10
            return new PrimeGeneratorResult(p, q);
        }
    }
}
