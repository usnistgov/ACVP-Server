using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Math.Helpers
{
    public static class BitArrayExtensions
    {
        public const int BITSINBYTE = 8;

        /* DEF (1/10/2017) -- 
            In BitArray...
            0-bit = least significant
            n-bit = most significant
            Which is reversed from the way CAVS thinks about it
            So we're going backwards here
        */
        public static BitArray BitShiftRight(this BitArray bArray) => new BitArray(bArray).RightShift(1);

        public static BitArray BitShiftRight(this BitArray bArray, int increment) => new BitArray(bArray).RightShift(increment);

        public static BitArray BitShiftLeft(this BitArray bArray) => new BitArray(bArray).LeftShift(1);

        public static BitArray BitShiftLeft(this BitArray bArray, int increment) => new BitArray(bArray).LeftShift(increment);

        public static byte[] ToBytes(this BitArray bArray)
        {
            byte[] bytes = new byte[(bArray.Length - 1) / BITSINBYTE + 1];
            bArray.CopyTo(bytes, 0);
            return bytes;
        }

        public static BitArray SubArray(this BitArray bArray, int startIndex, int length)
        {
            //if (startIndex + length > bArray.Count)
            //{
            //    throw new IndexOutOfRangeException();
            //}

            var copy = new BitArray(length);
            for (int i = startIndex, j = 0; j < length; i++, j++)
            {
                copy[j] = bArray[i];
            }
            return copy;
        }

        public static BitArray Concatenate(this BitArray bArray, BitArray bitsToAdd)
        {
            var boolArray = new bool[bArray.Length + bitsToAdd.Length];
            bArray.CopyTo(boolArray, 0);
            bitsToAdd.CopyTo(boolArray, bArray.Length);

            return new BitArray(boolArray);
        }

        public static String ToBinaryString(this BitArray bArray)
        {
            var sb = new StringBuilder();
            bool[] boolArray = new bool[bArray.Length];
            bArray.CopyTo(boolArray, 0);
            Array.ForEach(boolArray, b => sb.Append(b ? "1" : "0"));
            return sb.ToString();
        }

        public static BitArray Reverse(this BitArray bArray)
        {
            return MsbLsbConversionHelpers.ReverseBitArrayBits(bArray);
        }

        /// <summary>
        /// Converts a <see cref="BitArray"/> to an int.
        /// </summary>
        /// <param name="bArray">The <see cref="BitArray"/> to convert.</param>
        /// <returns>Integer representation of <see cref="bArray"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <see cref="bArray"/> is greater than 32 bits.</exception>
        public static int ToInt(this BitArray bArray)
        {
            if (bArray.Count > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(bArray));
            }

            var value = 0;
            for (var i = 0; i < bArray.Length; i++)
            {
                if (bArray[i])
                {
                    value += 1 << i;
                }
            }

            return value;
        }
    }
}
