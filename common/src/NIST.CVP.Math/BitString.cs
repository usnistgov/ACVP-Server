using NIST.CVP.Math.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Helper = NIST.CVP.Math.Helpers.MsbLsbConversionHelpers;

namespace NIST.CVP.Math
{

    /// <summary>
    /// Bit and Byte functions manipulation functions.
    /// NOTE:
    ///     Input/Output of bits is always:
    ///         LSb to MSb - least significant bit first (index 0), most significant bit last (last index)
    ///     Everything else (bytes, hex, etc):
    ///         MSB to LSB - most significant Byte first (index 0), least significiant Byte last (last index)
    /// </summary>
    public class BitString
    {
        public const int BYTESPERDIGIT = 4;
        public const int BITSINBYTE = 8;
        private readonly BitArray _bits;

        /// <summary>
        /// In LSb
        /// </summary>
        public BitArray Bits
        {
            get { return _bits; }
        }

        public int BitLength
        {
            get { return _bits.Length; }
        }

        #region Constructors
        public BitString(int bitCount)
        {
            _bits = new BitArray(bitCount);
        }

        /// <summary>
        /// Create <see cref="BitString"/> expecting <see cref="byte[]"/> in Most Significant Byte (MSB) order.
        /// </summary>
        /// <param name="msBytes">The MSB bytes to use in the LSb <see cref="BitString"/></param>
        public BitString(byte[] msBytes)
        {
            _bits = Helper.MostSignificantByteArrayToLeastSignificantBitArray(msBytes);
        }

        /// <summary>
        /// Create <see cref="BitString"/> expecting <see cref="BitArray"/> in Least Signficant bit (LSb) order.
        /// </summary>
        /// <param name="bits">The LSb bits to use in the <see cref="BitString"/></param>
        public BitString(BitArray bits)
        {
            _bits = bits;
        }

        public BitString(BigInteger bigInt, int bitLength = 0)
        {
            byte[] bytesInLSB = bigInt.ToByteArray();

            if (bytesInLSB.Length * BITSINBYTE < bitLength)
            {
                BitArray bytesAsBitsLsb = new BitArray(bytesInLSB);
                BitArray holdLsb = new BitArray(bitLength);
                for (int i = 0; i < bytesAsBitsLsb.Length; i++)
                {
                    holdLsb[i] = bytesAsBitsLsb[i];
                }

                _bits = holdLsb;
            }
            else
            {
                _bits = new BitArray(bytesInLSB);
            }
        }

        /// <summary>
        /// Create a <see cref="BitString"/> using MSB hex.
        /// </summary>
        /// <param name="hexMSB">The MSB hexadecimal string</param>
        public BitString(string hexMSB)
        {
            
            if (string.IsNullOrEmpty(hexMSB))
            {
                _bits = new BitArray(0);
                return;
            }

            // TODO: Currently hex string must be an even length (with spaces stripped).
            // Should the string left pad with a 0 to make it even and not throw an exception?
            // Or should immediately throw if hex is odd (w/o spaces)?
            hexMSB = hexMSB.Replace(" ", "");
            int numberChars = hexMSB.Length;
            byte[] bytesInMSB = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytesInMSB[i / 2] = Convert.ToByte(hexMSB.Substring(i, 2), 16);
            }
                

            _bits = Helper.MostSignificantByteArrayToLeastSignificantBitArray(bytesInMSB);
        }
        #endregion Constructors

        public override bool Equals(object obj)
        {
            var otherBitString = obj as BitString;
            if (otherBitString == null)
            {
                return false;
            }

            if (this.BitLength != otherBitString.BitLength)
            {
                return false;
            }

            var copiedBits = new BitArray(this.Bits);
            var comparison = copiedBits.Xor(otherBitString.Bits);

            foreach (bool val in comparison)
            {
                if (val)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns bytes based on <see cref="Bits"/> in MSB.
        /// </summary>
        /// <remarks>
        /// Bytes are by default in Most Significant Byte order.  
        /// If true is provided to function, returned in Least Significant Byte order.
        /// </remarks>
        /// <param name="reverseBytes">Should the bytes be reverse in the array?  (Changes from MSB to LSB)</param>
        /// <returns><see cref="byte[]"/> of <see cref="Bits"/></returns>
        public byte[] ToBytes(bool reverseBytes = false)
        {
            byte[] bytes = new byte[(Bits.Length - 1) / BITSINBYTE + 1];
            //_bits.CopyTo(bytes, 0); This would be nice, but it is not supported in .Net Core 1.0.1

            int byteIndex = 0;

            for (int bit = 0; bit < Bits.Length; bit++)
            {
                if (Bits[bit])
                {
                    bytes[byteIndex] |= (byte)(1 << (bit % BITSINBYTE));
                }

                // New byte when bit (+1 since start index of 0) mod 8 = 0
                if (bit > 0 && (bit + 1) % BITSINBYTE == 0)
                {
                    byteIndex++;
                }
            }

            // Note bytes are currently in LSB, 
            // class inputs/outputs byte arrays in MSB by default, 
            //  so if reverse is specified, return as is, 
            //  otherwise reverse the LSB to get MSB and return that
            if (reverseBytes)
            {
                return bytes;
            }
            else
            {
                return bytes.Reverse().ToArray();
            }
        }

        public bool Set(int bitIndex, bool value)
        {
            if ((bitIndex < 0) || (bitIndex >= BitLength))
            {
                return false;
            }
            _bits[bitIndex] = value;

            return true;
        }

        public static BitString To64BitString(long value)
        {
            var bytesInLSB = BitConverter.GetBytes(value);
            var bitArrayLSb = Helper.LeastSignificantByteArrayToLeastSignificantBitArray(bytesInLSB);

            return new BitString(bitArrayLSb);
        }

        public static BitString XOR(BitString left, BitString right)
        {
            // Pad shorter BitString with 0s to match longer BitString length
            BitString.PadShorterBitStringWithZeroes(ref left, ref right);
            BitArray xorArray = left.Bits.Xor(right.Bits);

            return new BitString(new BitArray(xorArray));
        }

        public BitString XOR(BitString comparisonBitString)
        {
            return XOR(this, comparisonBitString);
        }
        
        /// <summary>
        /// Returns <see cref="Bits"/> as a string in MSb
        /// </summary>
        /// <returns>string representation of <see cref="Bits"/></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int bit = 0; bit < BitLength; bit++)
            {
                builder.Append(Bits[bit] ? "1" : "0");

                // Add a space to the builder if the bit is a multiple of 8, but not the last character
                if ((bit + 1) % BITSINBYTE == 0
                    && bit > 0
                    && (bit + 1) != Bits.Length)
                {
                    builder.Append(" ");
                }
            }

            // Note: reversing the string in order to represent a "most significant bit" order. 
            // e.g. "3" is written as "00000011" rather than "1100000".
            return new string(builder.ToString().Reverse().ToArray());
        }

        public BitString ConcatenateBits(BitString bitsToAppend)
        {
            return BitString.ConcatenateBits(this, bitsToAppend);
        }

        /// <summary>
        /// Concatenates two <see cref="BitString"/>.
        /// </summary>
        /// <example>
        ///     mostSignificantBits = "0010" (4)
        ///     leastSignificantBits = "1000" (1)
        ///     result = 10000010 (65)
        /// </example>
        /// <param name="mostSignificantBits">The bits that will be most significant after concatenation.</param>
        /// <param name="leastSignificantBits">The bits that will be least significant after concatenation.</param>
        /// <returns>The concatenated <see cref="BitString"/></returns>
        public static BitString ConcatenateBits(BitString mostSignificantBits, BitString leastSignificantBits)
        {
            bool[] bits = new bool[mostSignificantBits.BitLength + leastSignificantBits.BitLength];

            for (int i = 0; i < leastSignificantBits.BitLength; i++)
            {
                bits[i] = leastSignificantBits.Bits[i];
            }

            for (int i = 0; i < mostSignificantBits.BitLength; i++)
            {
                bits[leastSignificantBits.BitLength + i] = mostSignificantBits.Bits[i];
            }

            return new BitString(new BitArray(bits));
        }

        public static BitString GetMostSignificantBits(int numBits, BitString bitString)
        {
            return BitString.Substring(bitString, bitString.BitLength - numBits, numBits);
        }

        public BitString GetMostSignificantBits(int numBits)
        {
            return BitString.GetMostSignificantBits(numBits, this);
        }

        public static BitString GetLeastSignificantBits(int numBits, BitString bitString)
        {
            return BitString.Substring(bitString, 0, numBits);
        }

        public BitString GetLeastSignificantBits(int numBits)
        {
            return BitString.GetLeastSignificantBits(numBits, this);
        }

        public override int GetHashCode()
        {
            return this.Bits.GetHashCode();
        }

        public BitString Substring(int startIndex, int numberOfBits)
        {
            return Substring(this, startIndex, numberOfBits);
        }

        public static BitString Substring(BitString bsToSub, int startIndex, int numberOfBits)
        {
            if ((startIndex > bsToSub.BitLength - 1) || startIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startIndex)} out of range");
            }
            if (startIndex + numberOfBits > bsToSub.BitLength)
            {
                throw new ArgumentOutOfRangeException($"does not contain enough elements to pull {numberOfBits} starting at {startIndex}");
            }

            bool[] newBits = new bool[numberOfBits];
            for (int i = 0; i < numberOfBits; i++)
            {
                newBits[i] = bsToSub.Bits[startIndex + i];
            }

            return new BitString(new BitArray(newBits));
        }

        public BigInteger ToBigInteger()
        {
            return new BigInteger(ToBytes(true));
        }

        public string ToHex()
        {
            if (BitLength == 0)
            {
                return "";
            }

            var bytes = ToBytes();

            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            for (int index = 0; index < bytes.Length; index++)
            {
                hex.AppendFormat("{0:x2}", bytes[index]);
            }

            return hex.ToString().ToUpper();
        }

        #region Private methods
        private static void PadShorterBitStringWithZeroes(ref BitString inputA, ref BitString inputB)
        {
            if (inputA.BitLength == inputB.BitLength)
            {
                return;
            }

            if (inputA.BitLength > inputB.BitLength)
            {
                inputB = PadShorterBitStringWithZeroes(inputA, inputB);
            }
            else
            {
                inputA = PadShorterBitStringWithZeroes(inputB, inputA);
            }
        }

        private static BitString PadShorterBitStringWithZeroes(BitString longerBitString, BitString shorterBitString)
        {
            BitArray newArray = new BitArray(longerBitString.BitLength);
            for (int i = 0; i < shorterBitString.BitLength; i++)
            {
                newArray[i] = shorterBitString.Bits[i];
            }

            return new BitString(newArray);
        }
        #endregion Private methods
    }
}
