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
    // B.3.6
    public class AllProbablePrimesWithConditionsGenerator : PrimeGeneratorBase, IPrimeGenerator
    {
        public AllProbablePrimesWithConditionsGenerator(EntropyProviderTypes type) : base(type) { }

        public AllProbablePrimesWithConditionsGenerator() { }

        public void AddEntropy(BitString bs)
        {
            _entropyProvider.AddEntropy(bs);
        }

        public void AddEntropy(BigInteger big)
        {
            _entropyProvider.AddEntropy(big);
        }

        public virtual PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq, xp1, xp2, xq1, xq2;

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
            xp1 = _entropyProvider.GetEntropy(_bitlens[0]).ToPositiveBigInteger();
            if (xp1.IsEven)
            {
                xp1++;
            }

            xp2 = _entropyProvider.GetEntropy(_bitlens[1]).ToPositiveBigInteger();
            if (xp2.IsEven)
            {
                xp2++;
            }

            p1 = xp1;
            while (!MillerRabin(nlen, p1, true))
            {
                p1 += 2;
            }

            p2 = xp2;
            while (!MillerRabin(nlen, p2, true))
            {
                p2 += 2;
            }

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
                xq1 = _entropyProvider.GetEntropy(_bitlens[2]).ToPositiveBigInteger();
                if (xq1.IsEven)
                {
                    xq1++;
                }

                xq2 = _entropyProvider.GetEntropy(_bitlens[3]).ToPositiveBigInteger();
                if (xq2.IsEven)
                {
                    xq2++;
                }

                q1 = xq1;
                while (!MillerRabin(nlen, q1, true))
                {
                    q1 += 2;
                }

                q2 = xq2;
                while (!MillerRabin(nlen, q2, true))
                {
                    q2 += 2;
                }

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

            var auxValues = new AuxiliaryPrimeGeneratorResult(xp1, xp2, xp, xq1, xq2, xq);
            return new PrimeGeneratorResult(p, q, auxValues);
        }
    }
}
