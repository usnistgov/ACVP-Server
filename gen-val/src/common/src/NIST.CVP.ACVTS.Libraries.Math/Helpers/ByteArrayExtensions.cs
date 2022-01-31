using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Threading.Tasks;

namespace NIST.CVP.ACVTS.Libraries.Math.Helpers
{
    /// <summary>
    /// bigInteger is little endian when dealing with byte-arrays, CAVS code thinks about byte array representations as big endian
    /// The BigInteger structure expects the individual bytes in a byte array to appear in little-endian order 
    /// (that is, the lower-order bytes of the value precede the higher-order bytes)
    /// also always want to return an array of the size of bArrayA, which means we might need to pad 
    /// </summary>
    public static class ByteArrayExtensions
    {
        public const byte MSBIT = 0x80;

        public static byte[] Add(this byte[] bArrayA, byte[] bArrayB)
        {

            if (bArrayB.Length > bArrayA.Length)
            {
                throw new ArgumentException("bArrayB.Length > bArrayA.Length");
            }

            if (bArrayB.Length != bArrayA.Length)
            {
                bArrayB = bArrayB.PadArrayToLength(bArrayA.Length);
            }
            var result = (new BigInteger(bArrayA) + new BigInteger(bArrayB)).ToByteArray();

            return result.PadArrayToLength(bArrayA.Length);
        }

        public static byte[] PadArrayToLength(this byte[] bArrayA, int padLength)
        {

            if (bArrayA.Length > padLength)
            {
                throw new ArgumentException("bArrayA.Length > padLength");
            }

            if (bArrayA.Length == padLength)
            {
                return bArrayA;
            }
            var paddedArray = new byte[padLength];
            for (int idx = 0; idx < bArrayA.Length; idx++)
            {
                paddedArray[idx] = bArrayA[idx];
            }

            return paddedArray;
        }

        public static byte[] SetOddParityBitInSuppliedBytes(this byte[] bArrayA)
        {
            for (int j = 0; j < bArrayA.Length; j++)
            {
                byte bmask = MSBIT;
                int bcount = 0;
                for (int k = 0; k < 8; k++)
                {
                    if ((bmask & bArrayA[j]) > 0)
                    {
                        bcount++;
                    }

                    bmask >>= 1;
                }
                if (bcount % 2 == 0)
                {
                    bArrayA[j] ^= 0x01;
                }

            }
            return bArrayA;
        }

        public static bool AllBytesHaveOddParity(this byte[] bArrayA)
        {
            for (int i = 0; i < bArrayA.Length; i++) /* do each byte */
            {
                int parityCount = 0;
                byte incopy = bArrayA[i];

                for (int j = 0; j < 8; j++) /* 8 bits in a DES byte */
                {
                    if ((incopy & 01) > 0)
                    {
                        parityCount++;
                    }
                    incopy >>= 1;
                }
                if ((parityCount & 01) == 0)
                {
                    return false; /* no odd parity on this byte */
                }
            }
            return true;
        }

        /// <summary>
        /// 1-based
        /// 1-bit (bNum == 0) always returns 0 
        /// </summary>
        /// <param name="bArrayA"></param>
        /// <param name="bnum"></param>
        /// <returns></returns>
        public static byte GetKeyBit(this byte[] bArrayA, int bnum)
        {
            byte b = bArrayA[bnum / 8];
            int shift = (byte)(0x08 - (bnum % 0x08));
            return (byte)(0x01 & ((int)b >> shift));
        }

        /// <summary>
        /// Set each byte in the provided array to the value of byteValue
        /// </summary>
        /// <param name="byteArray">The byteArray to set values on.</param>
        /// <param name="byteValue">The value to set within the byte array.</param>
        public static byte[] SetEachByteToValue(this byte[] byteArray, byte byteValue)
        {
            for (var i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = byteValue;
            }

            return byteArray;
        }
    }
}
