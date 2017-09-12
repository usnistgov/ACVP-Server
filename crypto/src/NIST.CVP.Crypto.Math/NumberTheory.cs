using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Math
{
    public static class NumberTheory
    {
        private static readonly Random800_90 _rand = new Random800_90();

        /// <summary>
        /// C.3.1 Probabilistic Primality Check, Miller-Rabin Algorithm
        /// </summary>
        /// <param name="w"></param>
        /// <param name="iterations"></param>
        /// <returns>True if probably prime. False if composite.</returns>
        public static bool MillerRabin(BigInteger n, int k)
        {
            if (n < 2)
            {
                return false;
            }

            if (n.IsEven)
            {
                return false;
            }

            var s = n - 1;
            while (s.IsEven)
            {
                s >>= 1;
            }

            for (var i = 0; i < k; i++)
            {
                var a = _rand.GetRandomBigInteger(n - 1) + 1;
                var temp = s;
                var mod = BigInteger.ModPow(a, temp, n);
                while (temp != n - 1 && mod != 1 && mod != n - 1)
                {
                    mod = (mod * mod) % n;
                    temp = temp * 2;
                }
                if (mod != n - 1 && temp.IsEven)
                {
                    return false;
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
        /// Find greatest common denominator of a and b, from page 606, Handbook of Applied Cryptography
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            BigInteger min, max;
            if(a <= b)
            {
                min = a;
                max = b;
            }
            else
            {
                min = b;
                max = a;
            }

            BigInteger g = 1;
            while(min.IsEven && max.IsEven)
            {
                min /= 2;
                max /= 2;
                g *= 2;
            }

            while(min != 0)
            {
                while (min.IsEven)
                {
                    min /= 2;
                }

                while (max.IsEven)
                {
                    max /= 2;
                }

                if (min >= max)
                {
                    min = (min - max) / 2;
                }
                else
                {
                    max = (max - min) / 2;
                }
            }

            return max * g;
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
