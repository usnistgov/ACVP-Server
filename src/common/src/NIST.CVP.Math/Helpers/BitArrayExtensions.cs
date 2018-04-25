using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

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
        public static BitArray BitShiftRight(this BitArray bArray)
        {
            var copy = new BitArray(bArray);
            for (int idx = 0; idx < copy.Length - 1; idx++)
            {
                copy[idx] = copy[idx + 1];
            }
            copy[copy.Length - 1] = false;

            return copy;
        }

        public static BitArray BitShiftLeft(this BitArray bArray)
        {
            var copy = new BitArray(bArray);
            for (int idx = copy.Length - 1; idx > 0; idx--)
            {
                copy[idx] = copy[idx -1];
            }
            copy[0] = false;

            return copy;
        }
        
        public static byte[] ToBytes(this BitArray bArray)
        {
            byte[] bytes = new byte[(bArray.Length - 1) / BITSINBYTE + 1];
            //_bits.CopyTo(bytes, 0); This would be nice, but it is not supported in .Net Core 1.0.1

            int byteIndex = 0;

            for (int bit = 0; bit < bArray.Length; bit++)
            {
                if (bArray[bit])
                {
                    bytes[byteIndex] |= (byte)(1 << (bit % BITSINBYTE));
                }

                // New byte when bit (+1 since start index of 0) mod 8 = 0
                if (bit > 0 && (bit + 1) % BITSINBYTE == 0)
                {
                    byteIndex++;
                }
            }
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
            foreach (var b in bArray)
            {
                sb.Append(Convert.ToBoolean(b) ? "1" : "0");
            }
            return sb.ToString();
        }

        public static BitArray Reverse(this BitArray bArray)
        {
            return MsbLsbConversionHelpers.ReverseBitArrayBits(bArray);
        }
    }
}
