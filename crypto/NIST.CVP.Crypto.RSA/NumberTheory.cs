using System.Numerics;
using Microsoft.Extensions.FileProviders.Physical;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    public static class NumberTheory
    {
        private static readonly Random800_90 _rand = new Random800_90();

        /// <summary>
        /// Probabilistic Primality Check, Miller-Rabin Algorithm
        /// </summary>
        /// <param name="w"></param>
        /// <param name="iterations"></param>
        /// <returns>True if probably prime. False if composite.</returns>
        public static bool MillerRabin(BigInteger w, int iterations)
        {
            var wLen = new BitString(w.ToByteArray()).BitLength;
            var a = 0;
            BigInteger m = 0;
            for (var i = 0; i < wLen; i++)
            {
                if (BigInteger.Pow(2, i) % (w - 1) == 0)
                {
                    a = i;
                    m = (w - 1) / BigInteger.Pow(2, i);
                }
            }

            for (var i = 1; i < iterations; i++)
            {
                var b = _rand.GetRandomBitString(wLen).ToPositiveBigInteger();
                if (b <= 1 || b >= w - 1)
                {
                    i--;
                    continue;
                }

                var z = BigInteger.ModPow(b, m, w);
                if (z == 1 || z == w - 1)
                {
                    continue;
                }

                for (var j = 1; j < a - 1; j++)
                {
                    z = BigInteger.ModPow(z, 2, w);
                    if (z == w - 1)
                    {
                        break;
                    }

                    if (z == 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Solve for b in the equation ab = 1 mod m
        /// </summary>
        /// <param name="a">value</param>
        /// <param name="m">modulo</param>
        /// <returns></returns>
        public static BigInteger ModularInverse(BigInteger a, BigInteger m)
        {
            BigInteger t = 0;
            BigInteger new_t = 1;
            BigInteger r = m;
            BigInteger new_r = a;
            BigInteger temp;

            while (new_r != 0)
            {
                var quotient = r / new_r;
                temp = new_t;
                new_t = t - quotient * new_t;
                t = temp;

                temp = new_r;
                new_r = r - quotient * new_r;
                r = temp;
            }

            // No inverse exists
            if (r > 1)
            {
                return 0;
            }

            // Ensure positivity
            if (t < 0)
            {
                t += m;
            }

            return t;
        }

        /// <summary>
        /// Find greatest common denominator of a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    
        /// <summary>
        /// Find least common multiple of a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger LCM(BigInteger a, BigInteger b)
        {
            return a * b / GCD(a, b);
        }

        /// <summary>
        /// Rounds up after dividing a by b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger CeilingDivide(BigInteger a, BigInteger b)
        {
            var result = a / b;
            var remainder = a % b;

            if (remainder != 0)
            {
                result++;
            }

            return result;
        }

        /// <summary>
        /// Calculates exponent a^b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger Pow(BigInteger a, BigInteger b)
        {
            // This is a bit lazy but should suffice. It's mainly just to solve some casting problems
            return BigInteger.Pow(a, (int)b);
        }

        /// <summary>
        /// Calculates exponent 2^exp quickly
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static BigInteger Pow2(int exp)
        {
            return BigInteger.One << exp;
        }
    }
}
