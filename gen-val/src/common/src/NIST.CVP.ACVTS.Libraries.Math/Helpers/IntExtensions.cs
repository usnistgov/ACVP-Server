using System;
using System.Linq;

namespace NIST.CVP.ACVTS.Libraries.Math.Helpers
{
    public static class IntExtensions
    {
        public static int CeilingDivide(this int a, int b)
        {
            // Modulo is slow, avoid it
            var result = a / b;
            if (result * b != a)
            {
                result++;
            }

            return result;
        }

        public static int FloorDivide(this int a, int b)
        {
            if ((a < 0) ^ (b < 0) && a % b != 0)
            {
                return (a / b - 1);
            }

            return a / b;
        }

        public static int IncrementOrReset(this int a, int min, int max, int increment = 1)
        {
            a++;
            if (a > max)
            {
                a -= max - min;
            }

            return a;
        }

        /// <summary>
        /// Takes the modulo of a value (or <see cref="int"/> expression) and ensures it is between [0, <paramref name="modulo"/> - 1]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulo"></param>
        /// <returns></returns>
        public static int PosMod(this int value, int modulo)
        {
            return (value % modulo + modulo) % modulo;
        }

        /// <summary>
        /// Gets the next number meeting the modulo (or the number itself if evenly divisible).
        /// </summary>
        /// <param name="value">The number to return or add to.</param>
        /// <param name="modulo">The desired modulo.</param>
        /// <returns>The value, or value + remainder of the value mod modulo.</returns>
        public static int ValueToMod(this int value, int modulo)
        {
            var remainder = value.PosMod(modulo);
            if (remainder == 0)
            {
                return value;
            }

            return value + modulo - remainder;
        }

        /// <summary>
        /// Get 4 bytes for int.
        /// </summary>
        /// <param name="value">The value to convert to bytes.</param>
        /// <returns>The MSB 4 bytes.</returns>
        public static byte[] GetBytes(this int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        /// <summary>
        /// Gets an integer as two bytes or 16 bits.
        /// </summary>
        /// <param name="value">The value to convert to bytes.</param>
        /// <returns>The MSB 2 bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the integer cannot be represented in two bytes.</exception>
        public static byte[] Get16Bits(this int value)
        {
            if (value > 65535)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} of {value} cannot be represented in two bytes.");
            }

            return BitConverter.GetBytes(value).Take(2).Reverse().ToArray();
        }

        /// <summary>
        /// Gets an integer as 1 byte or 8 bits.
        /// </summary>
        /// <param name="value">The value to convert to a byte.</param>
        /// <returns>The MSB 1 byte.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the integer cannot be represented in a single byte.</exception>
        public static byte[] Get8Bits(this int value)
        {
            if (value > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(value)} of {value} cannot be represented in one byte.");
            }

            return BitConverter.GetBytes(value).Take(1).Reverse().ToArray();
        }
    }
}
