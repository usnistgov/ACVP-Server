using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Math
{
    public static class NumberTheory
    {
        /// <summary>
        /// C.3.1 Probabilistic Primality Check, Miller-Rabin Algorithm
        /// </summary>
        /// <param name="n">Number</param>
        /// <param name="k">Iteraions</param>
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

            var rand = new Random800_90();
            for (var i = 0; i < k; i++)
            {
                var a = rand.GetRandomBigInteger(n - 1) + 1;
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
        /// Find greatest common denominator of a and b, from page 606, Handbook of Applied Cryptography
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            return BigInteger.GreatestCommonDivisor(a, b);
        }

        /// <summary>
        /// Find least common multiple of a and b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger LCM(BigInteger a, BigInteger b)
        {
            return a * b / BigInteger.GreatestCommonDivisor(a, b);
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
