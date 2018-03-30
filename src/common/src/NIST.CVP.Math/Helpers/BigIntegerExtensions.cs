using System.Numerics;

namespace NIST.CVP.Math.Helpers
{
    public static class BigIntegerExtensions
    {
        /// <summary>
        /// Takes the modulo of a value (or <see cref="BigInteger"/> expression) and ensures it is between [0, <paramref name="modulo"/> - 1]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static BigInteger PosMod(this BigInteger value, BigInteger modulo)
        {
            return ((value % modulo) + modulo) % modulo;
        }

        /// <summary>
        /// Determines the EXACT number of bits used in a <see cref="BigInteger"/> by converting the value to a <see cref="BitString"/> 
        /// and such that the MSbit is a 1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ExactBitLength(this BigInteger value)
        {
            var bs = new BitString(value);

            // Walk from the MSb end towards the LSb end and stop when a '1' is found
            for (var i = bs.BitLength - 1; i >= 0; i--)
            {
                if (bs.Bits[i] == true)
                {
                    return i + 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Returns the EXACT number of bits needed to represent the <see cref="BigInteger"/> value. The MSbit is always a 1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static BitString ExactBitString(this BigInteger value)
        {
            var bs = new BitString(value);
            var length = value.ExactBitLength();

            return bs.GetLeastSignificantBits(length);
        }

        /// <summary>
        /// Gets a specific bit where 0 is the LSbit
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool GetBit(this BigInteger value, int index)
        {
            var shifted = value >> index;
            return !shifted.IsEven;
        }

        /// <summary>
        /// returns the hex representation of a <see cref="BigInteger"/>
        /// </summary>
        /// <param name="value">The value to return as hex</param>
        /// <param name="zeroAsEmpty">Should <see cref="BigInteger.Zero"/> be represented as 
        /// an empty hex string (true) 
        /// or "00" (false)</param>
        /// <returns></returns>
        public static string ToHex(this BigInteger value, bool zeroAsEmpty = true)
        {
            if (value.IsZero)
            {
                return zeroAsEmpty ? string.Empty : "00";
            }

            return new BitString(value).ToHex();
        }

        /// <summary>
        /// Rounds up after dividing a by b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger CeilingDivide(this BigInteger a, BigInteger b)
        {
            var result = a / b;

            if (result * b != a)
            {
                result++;
            }

            return result;
        }

        /// <summary>
        /// Solve for b in the equation ab = 1 mod m
        /// </summary>
        /// <param name="a">value</param>
        /// <param name="m">modulo</param>
        /// <returns></returns>
        public static BigInteger ModularInverse(this BigInteger a, BigInteger m)
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
    }
}
