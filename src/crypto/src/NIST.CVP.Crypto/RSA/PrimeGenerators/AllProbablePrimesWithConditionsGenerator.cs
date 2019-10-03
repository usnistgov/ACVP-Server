using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System.Numerics;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    public class AllProbablePrimesWithConditionsGenerator : PrimeGeneratorBase
    {
        public AllProbablePrimesWithConditionsGenerator(IEntropyProvider entropyProvider, PrimeTestModes primeTestMode) : 
            base(entropyProvider: entropyProvider, primeTestMode: primeTestMode) { }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            BigInteger p, p1, p2, q, q1, q2, xp, xq, xp1, xp2, xq1, xq2;

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
            if (!RsaKeyHelper.IsValidExponent(e))
            {
                return new PrimeGeneratorResult(RsaKeyHelper.InvalidExponentMessage);
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
            //var xp1Rand = _entropyProvider.GetEntropy(_bitlens[0]);
            //xp1Rand.Set(xp1Rand.BitLength - 1, true);       // Set MSb to true
            //xp1Rand.Set(0, true);                           // Set LSb to true
            //xp1 = xp1Rand.ToPositiveBigInteger();
            xp1 = _entropyProvider.GetEntropy(_bitlens[0]).ToPositiveBigInteger();
            if (xp1.IsEven)
            {
                xp1++;
            }

            //var xp2Rand = _entropyProvider.GetEntropy(_bitlens[1]);
            //xp2Rand.Set(xp2Rand.BitLength - 1, true);       // Set MSb to true
            //xp2Rand.Set(0, true);                           // Set LSb to true
            //xp2 = xp2Rand.ToPositiveBigInteger();
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

            var pResult = ProbablePrimeFactor(p1, p2, nlen, e, securityStrength);
            if (!pResult.Success)
            {
                return new PrimeGeneratorResult($"Failed to generate p: {pResult.ErrorMessage}");
            }
            p = pResult.Prime;
            xp = pResult.XPrime;

            // 5
            do
            {
                //var xq1Rand = _entropyProvider.GetEntropy(_bitlens[2]);
                //xq1Rand.Set(xq1Rand.BitLength - 1, true);       // Set MSb to true
                //xq1Rand.Set(0, true);                           // Set LSb to true
                //xq1 = xq1Rand.ToPositiveBigInteger();
                xq1 = _entropyProvider.GetEntropy(_bitlens[2]).ToPositiveBigInteger();
                if (xq1.IsEven)
                {
                    xq1++;
                }

                //var xq2Rand = _entropyProvider.GetEntropy(_bitlens[3]);
                //xq2Rand.Set(xq2Rand.BitLength - 1, true);       // Set MSb to true
                //xq2Rand.Set(0, true);                           // Set LSb to true
                //xq2 = xq2Rand.ToPositiveBigInteger();
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

                var qResult = ProbablePrimeFactor(q1, q2, nlen, e, securityStrength);
                if (!qResult.Success)
                {
                    return new PrimeGeneratorResult($"Failed to generate q: {qResult.ErrorMessage}");
                }
                q = qResult.Prime;
                xq = qResult.XPrime;

                // 6
            } while (BigInteger.Abs(xp - xq) <= NumberTheory.Pow2(nlen / 2 - 100) ||
                     BigInteger.Abs(p - q)   <= NumberTheory.Pow2(nlen / 2 - 100));

            var auxValues = new AuxiliaryResult{ XP1 = xp1, XP2 = xp2, XP = xp, XQ1 = xq1, XQ2 = xq2, XQ = xq };
            var primePair = new PrimePair {P = p, Q = q};
            return new PrimeGeneratorResult(primePair, auxValues);
        }
    }
}
