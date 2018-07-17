using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System.Text;

namespace NIST.CVP.Crypto.SHA3
{
    public class SHA3DerivedHelpers
    {
        // All functions are defined in SP800-185

        public static BitString EncodeString(BitString message)
        {
            var messageLen = message.BitLength;
            var messageLenBitString = IntToBitString(messageLen);
            if (messageLen == 0)
            {
                messageLenBitString = BitString.Zeroes(8);
            }
            messageLenBitString = LeftEncode(messageLenBitString);
            return BitString.ConcatenateBits(messageLenBitString, message);
        }
        
        public static BitString LeftEncode(BitString message)
        {
            var messageLen = message.BitLength / 8;
            var messageLenBitString = IntToBitString(messageLen);
            return BitString.ConcatenateBits(messageLenBitString, message);
        }
        
        public static BitString RightEncode(BitString message)
        {
            var messageLen = message.BitLength / 8;
            var messageLenBitString = IntToBitString(messageLen);
            return BitString.ConcatenateBits(message, messageLenBitString);
        }
        
        public static BitString Bytepad(BitString input, BitString w)
        {
            var z = BitString.ConcatenateBits(LeftEncode(w), input);
            while (z.BitLength % 8 != 0)
            {
                z = BitString.ConcatenateBits(z, BitString.Zero());
            }
            while ((z.BitLength / 8) % w.ToPositiveBigInteger() != 0)
            {
                z = BitString.ConcatenateBits(z, BitString.Zeroes(8));
            }
            return z;
        }

        public static BitString SubString(BitString X, int start, int end)
        {
            if (start >= end || start >= X.BitLength)
            {
                return new BitString(0);
            }
            else if (end <= X.BitLength)
            {
                return X.MSBSubstring(start, end - start);
            }
            else
            {
                return X.MSBSubstring(start, X.BitLength - start);
            }
        }
        
        protected static BitString StringToHex(string words)
        {
            var ba = Encoding.ASCII.GetBytes(words);
            return new BitString(ba);
        }

        protected static BitString IntToBitString(int num)
        {
            return new BitString(new System.Numerics.BigInteger(num));
        }

        // use for KMAC and TupleHash bit oriented messages when concatenating with encoded lengths
        // this occurs when the leftBits are bitoriented
        protected static BitString SafeConcatenation(BitString leftBits, BitString rightBits)
        {
            var result = BitString.ConcatenateBits(ConvertEndianness(leftBits), ConvertEndianness(rightBits));
            return ConvertEndianness(result);
            //result.Set(0, false);
            //return result.ConcatenateBits(BitString.One());
        }

        // needed for KMAC and TupleHash bit oriented messages
        private static BitString ConvertEndianness(BitString message)
        {
            // This is kinda gross... The message input is in the correct byte order but reversed bit order
            // So we must reverse the bits, then reverse the bytes to put everything in the correct order
            //
            // For a small example... 60 01 (hex) = 0110 0001 (binary)
            //    should turn into    06 80 (hex) = 0110 1000 (binary

            var messageLen = message.BitLength;

            // Convert to big endian byte order but little endian bit order
            var reversedBits = MsbLsbConversionHelpers.ReverseBitArrayBits(message.Bits);
            var normalizedBits = MsbLsbConversionHelpers.ReverseByteOrder(new BitString(reversedBits).ToBytes());

            // After the byte conversion make sure the result is the correct length
            // The constructor here handles this for us
            message = new BitString(normalizedBits);
            var hex = message.ToHex();
            message = new BitString(hex, messageLen, false);

            return message;
        }
    }
}
