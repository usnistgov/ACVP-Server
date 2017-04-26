using System;
using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA.PrimeGenerators
{
    // B.3.3
    public class RandomProbablePrimeGenerator : PrimeGeneratorBase
    {
        public override PrimeGeneratorResult GeneratePrimes(int nlen, BigInteger e, BitString seed)
        {
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
            while (true)
            {
                while (true)
                {
                    // 4.2
                    p = _rand.GetRandomBitString(nlen / 2).ToPositiveBigInteger();

                    // 4.3
                    if (p.IsEven)
                    {
                        p++;
                    }

                    // 4.4
                    if (!(p < bound))
                    {
                        break;
                    }
                }
                
                // 4.5
                if (NumberTheory.GCD(p - 1, e) == 1)
                {
                    // primality test
                    break;
                }

                // 4.6, 4.7
                i++;
                if (i >= 5 * (nlen / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for p");
                }
            }

            // 5, 5.1
            i = 0;
            BigInteger q;
            while (true)
            {
                while (true)
                {
                    // 5.2
                    q = _rand.GetRandomBitString(nlen / 2).ToPositiveBigInteger();

                    // 5.3
                    if (q.IsEven)
                    {
                        q++;
                    }

                    // 5.4
                    if (!(BigInteger.Abs(p - q) <= BigInteger.Pow(2, nlen / 2 - 100)))
                    {
                        break;
                    }

                    // 5.5
                    if (!(q < bound))
                    {
                        break;
                    }
                }    
                
                // 5.6
                if (NumberTheory.GCD(q - 1, e) == 1)
                {
                    // primality test
                    break;
                }

                // 5.7, 5.8
                i++;
                if (i >= 5 * (nlen / 2))
                {
                    return new PrimeGeneratorResult("Too many iterations for q");
                }
            }

            return new PrimeGeneratorResult(p, q);
        }
    }
}
