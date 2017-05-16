using System;
using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    // B.3.3
    public class RandomProbablePrimeGenerator : PrimeGeneratorBase
    {
        // These are strings because you can't default a BigInteger
        public PrimeGeneratorResult GenerateTestPrimes(int nlen, BigInteger e, bool kat, string pRandom = "", string qRandom = "")
        {
            var givenP = new BitString(pRandom).ToPositiveBigInteger();
            var givenQ = new BitString(qRandom).ToPositiveBigInteger();

            // 1
            if (nlen != 2048 && nlen != 3072)
            {
                return new PrimeGeneratorResult("Incorrect nlen, must be 2048, 3072");
            }

            // 2
            if (e <= BigInteger.Pow(2, 16) || e >= BigInteger.Pow(2, 256) || e.IsEven)
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

            // 4, 4.1
            var i = 0;
            BigInteger p;
            var bound = nlen == 3072 ? _root2Mult2Pow1536Minus1 : _root2Mult2Pow1024Minus1;
            var millerRabinRounds = nlen == 3072 ? 64 : 56;
            do
            {
                do
                {
                    // 4.2
                    if (kat)
                    {
                        if (p == givenP)
                        {
                            return new PrimeGeneratorResult("Given p less than sqrt(2) * 2 ^ (n/2) - 1, so get a new random number.");
                        }
                        p = givenP;
                    }
                    else
                    {
                        p = _entropyProvider.GetEntropy(nlen / 2).ToPositiveBigInteger();
                    }

                    // 4.3
                    if (p.IsEven)
                    {
                        p++;
                    }

                    // 4.4
                } while (p < bound);

                // 4.5
                if (NumberTheory.GCD(p - 1, e) == 1)
                {
                    if (NumberTheory.MillerRabin(p, millerRabinRounds))
                    {
                        break;
                    }
                }

                // 4.6, 4.7
                i++;
                if (i >= 5 * (nlen / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for p");
                }

                if (kat)
                {
                    return new PrimeGeneratorResult("Given p is not prime");
                }
            } while (!kat);

            // 5, 5.1
            i = 0;
            BigInteger q;
            do
            {
                do
                {
                    // 5.2
                    if (kat)
                    {
                        if (q == givenQ)
                        {
                            return new PrimeGeneratorResult("Given q less than sqrt(2) * 2 ^ (n/2) - 1, so get a new random number.");
                        }
                        q = givenQ;
                    }
                    else
                    {
                        q = _entropyProvider.GetEntropy(nlen / 2).ToPositiveBigInteger();
                    }

                    // 5.3
                    if (q.IsEven)
                    {
                        q++;
                    }

                    // 5.4
                    // 5.5
                } while (BigInteger.Abs(p - q) <= NumberTheory.Pow2(nlen / 2 - 100) && q < bound);

                // 5.6
                if (NumberTheory.GCD(q - 1, e) == 1)
                {
                    if (NumberTheory.MillerRabin(q, millerRabinRounds))
                    {
                        break;
                    }
                }

                // 5.7, 5.8
                i++;
                if (i >= 5 * (nlen / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for q");
                }

                if (kat)
                {
                    return new PrimeGeneratorResult("Given q is not prime");
                }
            } while (!kat);

            return new PrimeGeneratorResult(p, q);
        }

        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
            return GenerateTestPrimes(nlen, e, false);
        }
    }
}
