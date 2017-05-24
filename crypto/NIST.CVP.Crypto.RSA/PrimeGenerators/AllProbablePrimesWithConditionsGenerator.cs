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
    public class AllProbablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        private int _bitlen1, _bitlen2, _bitlen3, _bitlen4;

        public AllProbablePrimesWithConditionsGenerator(EntropyProviderTypes type) : base(type) { }

        public void SetBitlens(int bitlen1, int bitlen2, int bitlen3, int bitlen4)
        {
            _bitlen1 = bitlen1;
            _bitlen2 = bitlen2;
            _bitlen3 = bitlen3;
            _bitlen4 = bitlen4;
        }

        public void AddEntropy(BitString bs)
        {
            _entropyProvider.AddEntropy(bs);
        }

        public void AddEntropy(BigInteger big)
        {
            _entropyProvider.AddEntropy(big);
        }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq;

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
            var xp1 = _entropyProvider.GetEntropy(_bitlen1).ToPositiveBigInteger();
            if (xp1.IsEven)
            {
                xp1++;
            }

            var xp2 = _entropyProvider.GetEntropy(_bitlen2).ToPositiveBigInteger();
            if (xp2.IsEven)
            {
                xp2++;
            }

            while (!NumberTheory.MillerRabin(xp1, 50))
            {
                xp1 += 2;
            }
            p1 = xp1;

            while (!NumberTheory.MillerRabin(xp2, 50))
            {
                xp2 += 2;
            }
            p2 = xp2;

            var pResult = ProbablePrimeFactor(p1, p2, nlen, e, security_strength);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult("Failed to generate p");
            }
            p = pResult.P;
            xp = pResult.XP;

            // 5
            do
            {
                var xq1 = _entropyProvider.GetEntropy(_bitlen3).ToPositiveBigInteger();
                if (xq1.IsEven)
                {
                    xq1++;
                }

                var xq2 = _entropyProvider.GetEntropy(_bitlen4).ToPositiveBigInteger();
                if (xq2.IsEven)
                {
                    xq2++;
                }

                while (!NumberTheory.MillerRabin(xq1, 50))
                {
                    xq1 += 2;
                }
                q1 = xq1;

                while (!NumberTheory.MillerRabin(xq2, 50))
                {
                    xq2 += 2;
                }
                q2 = xq2;

                var qResult = ProbablePrimeFactor(q1, q2, nlen, e, security_strength);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult("Failed to generate q");
                }
                q = qResult.P;
                xq = qResult.XP;

                // 6
            } while (BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(nlen / 2 - 100) ||
                     BigInteger.Abs(p - q)   <= NumberTheory.Pow2(nlen / 2 - 100));

            return new PrimeGeneratorResult(p, q);
        }
    }
}
