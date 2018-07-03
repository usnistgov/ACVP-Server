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

        /// <summary>
        /// Converts BigInteger to BitString with proper byte orientation
        /// </summary>
        /// <param name="bigInt"></param>
        /// <param name="bitLength"></param>
        public BitString(BigInteger bigInt, int bitLength = 0, bool allowRemoval = true)
        {
            byte[] bytesInLSB = bigInt.ToByteArray();

            // Sometimes, BigInteger -> byte[] adds an empty byte on the end to help
            // distinguish between two's complement for positive and negative.
            // Whenever it appears, we can just remove it safely.
            if (allowRemoval)
            {
                if (bytesInLSB[bytesInLSB.Length - 1] == 0)
                {
                    var list = bytesInLSB.ToList();
                    list.RemoveAt(list.Count - 1);
                    bytesInLSB = list.ToArray();
                }
            }

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
        /// <param name="bitLength">The length of the resulting <see cref="BitString"/> by taking that amount of MSBs</param>
        /// <param name="truncateBitsFromEndOfLastByte">When the bitLength is not a multiple of 8, the hex needs to be truncated in the last byte. This parameter determines which side of the last byte is truncated</param>
        public BitString(string hexMSB, int bitLength = -1, bool truncateBitsFromEndOfLastByte = true)
        {
            if (string.IsNullOrEmpty(hexMSB) || bitLength == 0)
            {
                _bits = new BitArray(0);
                return;
            }

            hexMSB = hexMSB.Replace(" ", "");
            int numberChars = hexMSB.Length;
            byte[] bytesInMSB = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytesInMSB[i / 2] = Convert.ToByte(hexMSB.Substring(i, 2), 16);
            }

            if(bitLength < 0)
            {
                _bits = Helper.MostSignificantByteArrayToLeastSignificantBitArray(bytesInMSB);
            }
            else
            {

                var bitsNeeded = System.Math.Min(bitLength, numberChars * BITSINBYTE / 2);

                var bitsInMSB = Helper.MostSignificantByteArrayToMostSignificantBitArray(bytesInMSB);
                var truncatedBits = new BitArray(bitsNeeded);

                if (truncateBitsFromEndOfLastByte)
                {
                    for (var i = 0; i < bitsNeeded; i++)
                    {
                        truncatedBits[i] = bitsInMSB[i];
                    }
                }
                else
                {
                    var firstBits = bitsNeeded - (bitsNeeded % 8);
                    for (var i = 0; i < firstBits; i++)
                    {
                        truncatedBits[i] = bitsInMSB[i];
                    }

                    var skippedBits = (bytesInMSB.Length * BITSINBYTE) - bitsNeeded;
                    for (var i = firstBits; i < bitsNeeded; i++)
                    {
                        truncatedBits[i] = bitsInMSB[i + skippedBits];
                    }
                }

                _bits = Helper.ReverseBitArrayBits(truncatedBits);
            }
        }

        // TODO this is a bit slow, can it be sped up?
        public BitString(byte[] bytesToConcatenate, int bitLengthToHit)
        {
            var maxBytes = bitLengthToHit.CeilingDivide(BITSINBYTE);
            var bytes = new byte[maxBytes];

            for (var i = 0; i < maxBytes; i++)
            {
                bytes[i] = bytesToConcatenate[i % bytesToConcatenate.Length];
            }

            var shortenedBits = Helper.MostSignificantByteArrayToLeastSignificantBitArray(bytes);
            if (shortenedBits.Length > bitLengthToHit)
            {
                shortenedBits = shortenedBits.SubArray(0, bitLengthToHit);
            }

            _bits = shortenedBits;
        }
        #endregion Constructors

        #region Conversions

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
            if (Bits.Length == 0)
            {
                return new byte[0];
            }
            
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

        public static BitString To64BitString(long value)
        {
            var bytesInLSB = BitConverter.GetBytes(value);
            var bitArrayLSb = Helper.LeastSignificantByteArrayToLeastSignificantBitArray(bytesInLSB);

            return new BitString(bitArrayLSb);
        }

        public static BitString To64BitString(ulong value)
        {
            var bytesInLSB = BitConverter.GetBytes(value);
            var bitArrayLSb = Helper.LeastSignificantByteArrayToLeastSignificantBitArray(bytesInLSB);

            return new BitString(bitArrayLSb);
        }

        public static BitString To32BitString(int value)
        {
            var bytesInLSB = BitConverter.GetBytes(value);
            var bitArrayInLsb = Helper.LeastSignificantByteArrayToLeastSignificantBitArray(bytesInLSB);

            return new BitString(bitArrayInLsb);
        }

        public static BitString To16BitString(short value)
        {
            var bytesInLSB = BitConverter.GetBytes(value);
            var bitArrayInLsb = Helper.LeastSignificantByteArrayToLeastSignificantBitArray(bytesInLSB);

            return new BitString(bitArrayInLsb);
        }

        /// <summary>
        /// Returns <see cref="Bits"/> as a string in MSb
        /// </summary>
        /// <returns>string representation of <see cref="Bits"/></returns>
        public override string ToString()
        {
            if (BitLength == 0)
            {
                return "";
            }

            var builder = new StringBuilder();
            var offset = BitLength % BITSINBYTE;

            for (var bit = 0; bit < BitLength; bit++)
            {
                // Add a space to the builder if the bit is a multiple of 8, but not the last character
                if (bit % BITSINBYTE == offset && bit > 0)
                {
                    builder.Append(" ");
                }

                builder.Append(Bits[bit] ? "1" : "0");
            }

            // Note: reversing the string in order to represent a "most significant bit" order. 
            // e.g. "3" is written as "00000011" rather than "11000000".
            return new string(builder.ToString().Reverse().ToArray());
        }

        public BigInteger ToBigInteger()
        {
            return new BigInteger(ToBytes(true));
        }

        public BigInteger ToPositiveBigInteger()
        {
            //var padding = BITSINBYTE - (BitLength % BITSINBYTE) + BITSINBYTE;
            var padding = BITSINBYTE - (BitLength % BITSINBYTE);

            // Add an empty byte (or more) to the beginning to get rid of two's complement
            var paddedBitString = ConcatenateBits(Zeroes(padding), this);
            return new BigInteger(paddedBitString.ToBytes(true));
        }

        public string ToHex()
        {
            if (BitLength == 0)
            {
                return "";
            }

            var bytes = new byte[] { };

            // Make a padded BitString if the length isn't % 8
            if (BitLength % 8 != 0)
            {
                var padding = BITSINBYTE - BitLength % BITSINBYTE;
                var paddedBS = new BitString(BitLength + padding);

                for (var i = 0; i < BitLength; i++)
                {
                    paddedBS.Set(i + padding, Bits[i]);
                }

                bytes = paddedBS.ToBytes();
            }
            else
            {
                bytes = ToBytes();
            }

            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            for (int index = 0; index < bytes.Length; index++)
            {
                hex.AppendFormat("{0:x2}", bytes[index]);
            }

            return hex.ToString().ToUpper();
        }

        /// <summary>
        /// Only for use in SHA3
        /// </summary>
        /// <returns>Normal hex, but the last byte is truncated to match little endian format</returns>
        public string ToLittleEndianHex()
        {
            if (BitLength == 0)
            {
                return "00";
            }

            var bytes = new byte[] { };

            // Make a padded BitString if the length isn't % 8
            if (BitLength % BITSINBYTE != 0)
            {
                var padding = BITSINBYTE - BitLength % BITSINBYTE;
                var extraBits = BitLength % BITSINBYTE;
                var lastBits = this.Substring(0, extraBits);

                var paddedBS = new BitString(0);
                if (BitLength < BITSINBYTE)
                {
                    paddedBS = BitString.Zeroes(padding);
                    paddedBS = BitString.ConcatenateBits(paddedBS, lastBits);
                }
                else
                {
                    var firstBits = this.Substring(extraBits, BitLength - extraBits);
                    paddedBS = BitString.ConcatenateBits(paddedBS, firstBits);

                    paddedBS = BitString.ConcatenateBits(paddedBS, BitString.Zeroes(padding));
                    paddedBS = BitString.ConcatenateBits(paddedBS, lastBits);
                }

                bytes = paddedBS.ToBytes();
            }
            else
            {
                bytes = ToBytes();
            }

            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            for (int index = 0; index < bytes.Length; index++)
            {
                hex.AppendFormat("{0:x2}", bytes[index]);
            }

            return hex.ToString().ToUpper();
        }

        /// <summary>
        /// Returns the MSb (or index of the LSb array) as a boolean value
        /// </summary>
        /// <param name="index">Index of the LSb BitArray to retrieve</param>
        /// <returns>Boolean of a specific bit.</returns>
        public bool ToBool(int index = -1)
        {
            if (index < 0)
            {
                index = Bits.Length - 1;
            }
            return Bits[index];
        }

        public BitString ToOddParityBitString()
        {
            var bitStringAsBytes = ToBytes();
            var oddParityBytes = bitStringAsBytes.SetOddParityBitInSuppliedBytes();
            return new BitString(oddParityBytes);
        }
        #endregion Conversions

        #region Logical Operators
        public static BitString XOR(BitString sourceLeft, BitString sourceRight)
        {
            // Deep copies to avoid accidental assignment
            BitString left = sourceLeft.GetDeepCopy();
            BitString right = sourceRight.GetDeepCopy();

            // Pad shorter BitString with 0s to match longer BitString length
            BitString.PadShorterBitStringWithZeroes(ref left, ref right);
            BitArray xorArray = left.Bits.Xor(right.Bits);

            return new BitString(new BitArray(xorArray));
        }

        public BitString XOR(BitString comparisonBitString)
        {
            return XOR(this, comparisonBitString);
        }

        public static BitString OR(BitString sourceLeft, BitString sourceRight)
        {
            // Deep copies to avoid accidental assignment
            BitString left = sourceLeft.GetDeepCopy();
            BitString right = sourceRight.GetDeepCopy();

            BitString.PadShorterBitStringWithZeroes(ref left, ref right);
            BitArray orArray = left.Bits.Or(right.Bits);

            return new BitString(new BitArray(orArray));
        }

        public BitString OR(BitString comparisonBitString)
        {
            return OR(this, comparisonBitString);
        }

        public static BitString AND(BitString sourceLeft, BitString sourceRight)
        {
            BitString left = sourceLeft.GetDeepCopy();
            BitString right = sourceRight.GetDeepCopy();

            BitString.PadShorterBitStringWithZeroes(ref left, ref right);
            BitArray andArray = left.Bits.And(right.Bits);

            return new BitString(new BitArray(andArray));
        }

        public BitString AND(BitString comparisonBitString)
        {
            return AND(this, comparisonBitString);
        }

        public static BitString NOT(BitString source)
        {
            BitString val = source.GetDeepCopy();
            return new BitString(val.Bits.Not());
        }

        public BitString NOT()
        {
            return NOT(this);
        }

        /// <summary>
        /// Rotates bits in the MSB direction. Rotate puts the bits that 'fall off' onto the end.
        /// </summary>
        /// <param name="shiftBitString">BitString to rotate</param>
        /// <param name="distance">Amount the bits to rotate.</param>
        /// <returns>Rotated BitString</returns>
        public static BitString MSBRotate(BitString shiftBitString, int distance)
        {
            var bits = shiftBitString.GetDeepCopy().GetBitsMSB();
            var bitLength = shiftBitString.BitLength;
            var newBits = new bool[bitLength];

            for (var i = 0; i < bitLength; i++)
            {
                newBits[i] = bits[(i + distance) % bitLength];
            }

            Array.Reverse(newBits);
            return new BitString(new BitArray(newBits));
        }

        public BitString MSBRotate(int distance)
        {
            return MSBRotate(this, distance);
        }

        /// <summary>
        /// Rotates bist in the LSB direction. Rotate puts the bits that 'fall off' onto the end.
        /// </summary>
        /// <param name="bStr"></param>
        /// <param name="distance">Amount the bits to rotate.</param>
        /// <returns></returns>
        public static BitString LSBRotate(BitString bStr, int distance)
        {
            var minDistance = distance % bStr.BitLength;
            return BitString.MSBRotate(bStr, bStr.BitLength - minDistance);
        }

        public BitString LSBRotate(int distance)
        {
            return BitString.LSBRotate(this, distance);
        }

        /// <summary>
        /// Shifts bits in the LSB direction. Shift adds 0s to the end.
        /// </summary>
        /// <param name="bStr"></param>
        /// <param name="distance">Amount of bits to shift.</param>
        /// <returns></returns>
        public static BitString LSBShift(BitString bStr, int distance)
        {
            var minDistance = System.Math.Min(bStr.BitLength, distance);
            var circleShift = BitString.MSBRotate(bStr, bStr.BitLength - minDistance);
            for (var i = 0; i < minDistance; i++)
            {
                circleShift.Set(bStr.BitLength - i - 1, false);
            }

            return circleShift;
        }

        public BitString LSBShift(int distance)
        {
            return BitString.LSBShift(this, distance);
        }

        public static BitString BitStringSubtraction(BitString larger, BitString smaller)
        {
            var largerVal = larger.ToPositiveBigInteger();
            var smallerVal = smaller.ToPositiveBigInteger();

            if (smallerVal > largerVal)
            {
                throw new ArgumentException("Unable to subtract, leads to negative value");
            }

            var result = largerVal - smallerVal;
            return new BitString(result, larger.BitLength);
        }

        public BitString BitStringSubtraction(BitString right)
        {
            return BitStringSubtraction(this, right);
        }

        /// <summary>
        /// Adds two bit strings together - e.g. "11" (3) + 111 (7) = 1010 (10).
        /// Similar to <see cref="AddWithModulo(NIST.CVP.Math.BitString,NIST.CVP.Math.BitString,int)"/>, but without truncation.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static BitString BitStringAddition(BitString left, BitString right)
        {
            left = left.GetDeepCopy();
            right = right.GetDeepCopy();

            PadShorterBitStringWithZeroes(ref left, ref right);

            int length = left.BitLength; // they are now of equal length, doesn't matter which is used
            int carry = 0;
            List<bool> bits = new List<bool>();

            // Add all bits one by one
            for (int i = 0; i < length; i++)
            {
                int firstBit = left.Bits[i] ? 1 : 0;
                int secondBit = right.Bits[i] ? 1 : 0;

                bool sum = (firstBit ^ secondBit ^ carry) == 1;

                bits.Add(sum);

                carry = (firstBit & secondBit) | (secondBit & carry) | (firstBit & carry);
            }

            // if overflow, then add a bit
            if (carry == 1)
            {
                bits.Add(true);
            }

            return new BitString(new BitArray(bits.ToArray()));
        }

        /// <summary>
        /// Adds two bit strings together - e.g. "11" (3) + 111 (7) = 1010 (10).
        /// Similar to <see cref="AddWithModulo(NIST.CVP.Math.BitString,NIST.CVP.Math.BitString,int)"/>, but without truncation.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public BitString BitStringAddition(BitString right)
        {
            return BitStringAddition(this, right);
        }

        /// <summary>
        /// Adds two BitStrings and truncates the result to fit within the limit.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="moduloPower">Amount of bits kept in the result.</param>
        /// <returns></returns>
        public static BitString AddWithModulo(BitString left, BitString right, int moduloPower)
        {
            var leftBits = left.GetDeepCopy().GetBitsMSB();
            var rightBits = right.GetDeepCopy().GetBitsMSB();
            var resultBits = new bool[moduloPower];
            var carryOver = false;

            for (var i = moduloPower - 1; i >= 0; i--)
            {
                // Lack of implicit conversion here is gross
                var bitResult = Convert.ToInt32(leftBits[i]) + Convert.ToInt32(rightBits[i]) + Convert.ToInt32(carryOver);

                carryOver = (bitResult >= 2);
                resultBits[i] = (bitResult % 2 == 1);
            }

            Array.Reverse(resultBits);

            return new BitString(new BitArray(resultBits));
        }

        public BitString AddWithModulo(BitString right, int moduloPower)
        {
            return AddWithModulo(this, right, moduloPower);
        }
        #endregion Logical Operators

        #region Getters and Setters
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

        /// <summary>
        /// Gets/Sets the byte at index specified.
        /// Index 0 is the most significant byte.
        /// </summary>
        /// <param name="index">The index to get/set (index 0 is most significant byte)</param>
        /// <returns></returns>
        public byte this[int index]
        {
            get
            {
                // Get bits for index (bits are in LSb, whereas bytes as MSB).
                // So byte index 0, is the last 8 bits of the BitArray
                BitString bits = this.Substring(BitLength - ((index + 1) * 8), 8);

                return bits.ToBytes().FirstOrDefault();
            }
            set
            {
                // Put the single byte in a byte array
                byte[] byteArray = new byte[1] { value };
                // convert that byte array to a bit array
                BitArray bits = new BitArray(byteArray);

                // For each bit, set that bit in this, 
                // noting that bits and bytes are in opposite endianness.
                for (int i = 0; i < bits.Length; i++)
                {
                    var bitToSet = BitLength - ((index + 1) * 8) + i;
                    this.Set(bitToSet, bits[i]);
                }
            }
        }

        public BitString GetDeepCopy()
        {
            return new BitString(new BitArray(_bits));
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
        #endregion Getters and Setters

        #region Concatenation
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
        #endregion Concatentation

        #region Substring
        /// <summary>
        /// Gets substring of a BitString from the LSB direction.
        /// </summary>
        /// <param name="bsToSub"></param>
        /// <param name="startIndex">Least significant bit is 0 index.</param>
        /// <param name="numberOfBits"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Takes a BitString and adds LSbs to make the BitString hit a byte boundry.  
        /// Returns the original BitString if already at a byte boundry.
        /// </summary>
        /// <param name="bs">The BitString to pad.</param>
        /// <returns></returns>
        public static BitString PadToNextByteBoundry(BitString bs)
        {
            return PadToModulus(bs, BITSINBYTE);
        }

        /// <summary>
        /// Takes a BitString and adds LSBs to make the BitString hit (BitString % modulus = 0)
        /// </summary>
        /// <param name="bs">The BitString to pad</param>
        /// <param name="modulus">The modulus to pad the bitstring such that BitString % modulusToHit = 0</param>
        /// <returns>The padded BitString</returns>
        public static BitString PadToModulus(BitString bs, int modulus)
        {
            if (bs.BitLength % modulus == 0)
            {
                return bs;
            }

            var bitsToAdd = (modulus - bs.BitLength % modulus);

            return bs.ConcatenateBits(new BitString(bitsToAdd));
        }

        public BitString Substring(int startIndex, int numberOfBits)
        {
            return Substring(this, startIndex, numberOfBits);
        }

        /// <summary>
        /// Gets a substring of bits from the MSB direction. 
        /// </summary>
        /// <param name="bsToSub"></param>
        /// <param name="startIndex">Start index from the MSB side. Most significant bit is index 0.</param>
        /// <param name="numberOfBits"></param>
        /// <returns></returns>
        public static BitString MSBSubstring(BitString bsToSub, int startIndex, int numberOfBits)
        {
            return Substring(bsToSub, bsToSub.BitLength - startIndex - numberOfBits, numberOfBits);
        }

        public BitString MSBSubstring(int startIndex, int numberOfBits)
        {
            return MSBSubstring(this, startIndex, numberOfBits);
        }
        #endregion Substring

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

        public override int GetHashCode()
        {
            return this.ToHex().GetHashCode();
        }

        public static BitString Zero()
        {
            return Zeroes(1);
        }

        public static BitString Zeroes(int length)
        {
            var bits = new BitArray(length);
            bits.SetAll(false);
            return new BitString(bits);
        }

        public static BitString One()
        {
            return Ones(1);
        }

        public static BitString Two()
        {
            return new BitString("80", 2);
        }

        public static BitString Ones(int length)
        {
            var bits = new BitArray(length);
            bits.SetAll(true);
            return new BitString(bits);
        }

        public static BitString ReverseByteOrder(BitString input)
        {
            return new BitString(MsbLsbConversionHelpers.ReverseByteOrder(input.ToBytes()));
        }

        /// <summary>
        /// Returns a zero length bitstring when passed a null, otherwise returns the bitstring
        /// </summary>
        /// <param name="bitString"></param>
        /// <returns></returns>
        public static BitString GetAtLeastZeroLengthBitString(BitString bitString)
        {
            if (bitString == null)
                return new BitString(0);

            return bitString;
        }

        public static bool IsZeroLengthOrNull(BitString bitString)
        {
            if (bitString == null)
            {
                return true;
            }

            return bitString.BitLength == 0;
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

        // Return an array of bits with MSB in the first index
        private bool[] GetBitsMSB()
        {
            bool[] MSBBits = new bool[BitLength];
            for (var i = 0; i < BitLength; i++)
            {
                MSBBits[i] = Bits[i];
            }

            Array.Reverse(MSBBits);
            return MSBBits;
        }
        #endregion Private methods
    }
}
