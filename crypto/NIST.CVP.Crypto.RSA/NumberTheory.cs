using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA
{
    public static class NumberTheory
    {
        /// <summary>
        /// Probabilistic Primality Check, Miller-Rabin Algorithm
        /// </summary>
        /// <returns></returns>
        public static bool MillerRabin()
        {
            return false;
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
    }
}
