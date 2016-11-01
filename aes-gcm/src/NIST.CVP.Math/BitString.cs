using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NIST.CVP.Math
{

    public class BitString
    {
        public const int BYTESPERDIGIT = 4;
        private readonly BitArray _bits;

        public BitArray Bits
        {
            get { return _bits; }
        }

        public int Length
        {
            get { return _bits.Length; }
        }

        public BitString(int bitCount)
        {
            _bits = new BitArray(bitCount);
        }

        public BitString(byte[] bytes)
        {
            _bits = new BitArray(bytes);
        }

        public BitString(BitArray bits)
        {
            _bits = bits;
        }

        public BitString(BigInteger bigInt, int bitLength = 0)
        {
            _bits = new BitArray(bigInt.ToByteArray());
            if (_bits.Length < bitLength)
            {
                _bits.Length = bitLength;
            }
        }

        public BitString(string hex)
        {
            hex = hex.Replace(" ", "");
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            _bits = new BitArray(bytes);
        }

        public override bool Equals(object obj)
        {
            var otherBitString = obj as BitString;
            if (otherBitString == null)
            {
                return false;
            }

            if (this.Length != otherBitString.Length)
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
        /// Returns bytes based on <see cref="Bits"/>.
        /// </summary>
        /// <remarks>
        /// Bytes are by default in Least Significant Byte order.  
        /// If true is provided to function, returned in Most Significant Byte order.
        /// </remarks>
        /// <param name="reverseBytes">Should the bytes be reverse in the array?  (Changes from LSB to MSB)</param>
        /// <returns><see cref="byte[]"/> of <see cref="Bits"/></returns>
        public byte[] ToBytes(bool reverseBytes = false)
        {
            byte[] bytes = new byte[(Bits.Length - 1) / 8 + 1];
            //_bits.CopyTo(bytes, 0); This would be nice, but it is not supported in .Net Core 1.0.1

            int byteIndex = 0;

            for (int bit = 0; bit < Bits.Length; bit++)
            {
                if (Bits[bit])
                {
                    bytes[byteIndex] |= (byte)(1 << (bit % 8));
                }

                // New byte when bit (+1 since start index of 0) mod 8 = 0
                if (bit > 0 && (bit + 1) % 8 == 0)
                {
                    byteIndex++;
                }
            }

            if (reverseBytes)
            {
                return bytes.Reverse().ToArray();
            }

            return bytes;
        }

        public bool Set(int bitIndex, bool value)
        {
            if ((bitIndex < 0) || (bitIndex >= Length))
            {
                return false;
            }
            _bits[bitIndex] = value;

            return true;
        }

        public static BitString To64BitString(int value)
        {
            ulong longValue = (ulong)value;
            byte[] bytes = new byte[8];

            // Performs a bitshift for the length of the 64 bit array.  
            // Each individual byte's value is the bitshift by index (i) * times bits in a byte (8)
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(longValue >> i * 8);
            }
            return new BitString(bytes);
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

        private static void PadShorterBitStringWithZeroes(ref BitString inputA, ref BitString inputB)
        {
            if (inputA.Length == inputB.Length)
            {
                return;
            }

            if (inputA.Length > inputB.Length)
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
            //int arrayOffset = longerBitString.Length - shorterBitString.Length;

            BitArray newArray = new BitArray(longerBitString.Length);
            for (int i = 0; i < shorterBitString.Length; i++)
            {
                //newArray[i + arrayOffset] = shorterBitString.Bits[i];
                newArray[i] = shorterBitString.Bits[i];
            }

            return new BitString(newArray);
        }

        /// <summary>
        /// Returns <see cref="Bits"/> as a string in MSb
        /// </summary>
        /// <returns>string representation of <see cref="Bits"/></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int bit = 0; bit < Length; bit++)
            {
                builder.Append(Bits[bit] ? "1" : "0");

                // Add a space to the builder if the bit is a multiple of 8, but not the last character
                if ((bit + 1) % 8 == 0
                    && bit > 0
                    && (bit + 1) != Bits.Length)
                {
                    builder.Append(" ");
                }
            }

            // Note: reversing the string in order to represent a "most significant bit" order. 
            // e.g. "3" is written as "00000011" rather than the actual internal BitArray order of "1100000".
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
        ///     (Note example written in MSb)
        ///     leftSideBits = "0010" (2)
        ///     rightSideBIts = "1000" (8)
        ///     result = 00101000 (40)
        /// </example>
        /// <param name="leftSideBits">The left side bits.</param>
        /// <param name="rightSideBits">The right side bits.</param>
        /// <returns>The concatenated <see cref="BitString"/></returns>
        public static BitString ConcatenateBits(BitString leftSideBits, BitString rightSideBits)
        {
            bool[] bits = new bool[leftSideBits.Length + rightSideBits.Length];

            for (int i = 0; i < rightSideBits.Length; i++)
            {
                bits[i] = rightSideBits.Bits[i];
            }

            for (int i = 0; i < leftSideBits.Length; i++)
            {
                bits[rightSideBits.Length + i] = leftSideBits.Bits[i];
            }

            return new BitString(new BitArray(bits));
        }

        public BitString Leftmost(int numBits)
        {
            return Substring(Length - numBits, numBits);
        }

        public BitString Rightmost(int numBits)
        {
            return Substring(0, numBits);
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
            if ((startIndex > bsToSub.Length - 1) || startIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startIndex)} out of range");
            }
            if (startIndex + numberOfBits > bsToSub.Length)
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
            return new BigInteger(ToBytes());

            //// Convert to a byte array of length that is a multiple of
            //// BYTESPERDIGIT

            //int nBytes = ToBytes().Length;
            //int nBytesBS = ToBytes().Length;
            //int ub = nBytes % BYTESPERDIGIT;
            //int dl = 0;
            //if (ub != 0)
            //{
            //    dl = BYTESPERDIGIT - ub;
            //    nBytesBS += dl;
            //}

            //// Make a copy of the byte array that can be reordered
            //byte[] bs = new byte[nBytesBS];
            //for (int i = nBytes - 1; i >= 0; --i)
            //{
            //    bs[i + dl] = ToBytes()[i];
            //}
            //for (int i = 0; i < dl; ++i)
            //{
            //    bs[i] = 0x00;
            //}

            //// Reorder the bytes
            //ToDigit(bs);

            //// Create BigInteger object from reordered bytes
            //var asInteger = new BigInteger(bs);

            //// Return the BigInteger object that holds the integer
            //return asInteger;
        }

        public string ToHex()
        {
            if (Length == 0)
            {
                return "";
            }

            var bytes = ToBytes();

            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            for (int index = 0; index < bytes.Length; index++)
            {
                //if (index > 0 && index % 4 == 0)
                //{
                //    hex.Append(" ");
                //}
                hex.AppendFormat("{0:x2}", bytes[index]);
            }

            return hex.ToString().ToUpper();
        }

        public void ToDigit(byte[] b)
        {

            byte[] swap = new byte[BYTESPERDIGIT];

            for (int i = 0; i < b.Length; i += BYTESPERDIGIT)
            {
                for (int j = 0; j < BYTESPERDIGIT; j++)
                {
                    if ((i + (BYTESPERDIGIT - j - 1)) < b.Length)
                    {
                        swap[j] = b[BYTESPERDIGIT - j - 1];
                    }
                    else
                    {
                        swap[j] = 0x00;
                    }
                }
                for (int j = 0; j < BYTESPERDIGIT; j++)
                {
                    if ((i + j) < b.Length)
                    {
                        b[j] = swap[j];
                    }
                    else
                    {
                        //bad???
                    }
                }

                //@@@Why???
                //bwin += BYTESPERDIGIT;
            }
        }
    }

}
