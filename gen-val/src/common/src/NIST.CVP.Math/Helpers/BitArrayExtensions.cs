using System;
using System.Collections;
using System.Text;

namespace NIST.CVP.Math.Helpers
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
    }
}
