using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NIST.CVP.Math.Helpers
{
    public static class MsbLsbConversionHelpers
    {
        public static byte[] ReverseByteOrder(byte[] bytes)
        {
            return bytes.Reverse().ToArray();
        }

        public static BitArray ReverseBitArrayBits(BitArray array)
        {
            BitArray copy = new BitArray(array);
            
            int length = copy.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = copy[i];
                copy[i] = copy[length - i - 1];
                copy[length - i - 1] = bit;
            }

            return copy;
        }

        public static BitArray MostSignificantByteArrayToLeastSignificantBitArray(byte[] msBytes)
        {
            // Get the LSB of the MSB byte array, so both bits and bytes are LS
            var leastSignificantByteArray = ReverseByteOrder(msBytes);

            // Create a BitArray with the Least Signficiant Byte array
            return new BitArray(leastSignificantByteArray);            
        }

        public static BitArray MostSignificantByteArrayToMostSignificantBitArray(byte[] msBytes)
        {
            // Get the most significant byte array as a least significant bit array
            var leastSignificantBitArray = MostSignificantByteArrayToLeastSignificantBitArray(msBytes);

            // switch the bits for a most significant bit array
            return ReverseBitArrayBits(leastSignificantBitArray);
        }

        public static BitArray LeastSignificantByteArrayToLeastSignificantBitArray(byte[] lsBytes)
        {
            return new BitArray(lsBytes);
        }

        public static BitArray LeastSignificantByteArrayToMostSignificantBitArray(byte[] lsBytes)
        {
            var lsBitArray = new BitArray(lsBytes);

            // the reverse of the LSb BitArray is a MSb BitArray
            return ReverseBitArrayBits(lsBitArray);
        }

        public static BitArray GetBitArrayFromStringOf1sAnd0s(string onesAndZeroes)
        {
            if (string.IsNullOrEmpty(onesAndZeroes))
            {
                throw new ArgumentNullException(nameof(onesAndZeroes));
            }

            // Remove spaces
            onesAndZeroes = onesAndZeroes.Replace(" ", "");

            // Check string is made up of only ones and zeroes
            Regex rgx = new Regex("[^01]");
            if (rgx.IsMatch(onesAndZeroes))
            {
                throw new ArgumentException($"{nameof(onesAndZeroes)} contains invalid characters.  Only spaces (' '), 0, and 1 should be present within string");
            }

            int length = onesAndZeroes.Length;

            bool[] boolArray = new bool[length];
            for (int i = 0; i < length; i++)
            {
                if (onesAndZeroes[i] == '0')
                {
                    boolArray[i] = false;
                }
                if (onesAndZeroes[i] == '1')
                {
                    boolArray[i] = true;
                }
            }

            return new BitArray(boolArray);
        }

        public static BitArray GetBitArrayFromStringOf1sAnd0sReversed(string onesAndZeroes)
        {
            return GetBitArrayFromStringOf1sAnd0s(onesAndZeroes).Reverse();
        }
    }
}
