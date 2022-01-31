using System.Text;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SHA
{
    public static class Sha3DerivedHelpers
    {
        #region SP800-185 Defined Functions
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

            if (messageLen == 0)
            {
                messageLen = 1;
                message = BitString.Zeroes(BitString.BITSINBYTE);
            }

            var messageLenBitString = IntToBitString(messageLen);
            return BitString.ConcatenateBits(messageLenBitString, message);
        }

        public static BitString RightEncode(BitString message)
        {
            var messageLen = message.BitLength / 8;

            if (messageLen == 0)
            {
                messageLen = 1;
                message = BitString.Zeroes(BitString.BITSINBYTE);
            }

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

        #endregion SP800-185 Defined Functions

        #region Helpers
        public static BitString StringToHex(string words)
        {
            var ba = Encoding.ASCII.GetBytes(words);
            return new BitString(ba);
        }

        public static BitString IntToBitString(int num)
        {
            return new BitString(new System.Numerics.BigInteger(num));
        }

        // this is needed when the leftBits are bitoriented
        public static BitString SafeConcatenation(BitString leftBits, BitString rightBits)
        {
            BitString result;
            if (leftBits.BitLength % 8 == 0)
            {
                result = BitString.ConcatenateBits(leftBits, rightBits);
                return result;
            }
            else
            {
                result = BitString.ConcatenateBits(ConvertEndianness(leftBits), ConvertEndianness(rightBits));
                return ConvertEndianness(result);
            }
        }

        private static BitString ConvertEndianness(BitString message)
        {
            // For a small example... 60 01 (hex) = 0110 0000 0000 0001 (binary)
            //    should turn into    06 80 (hex) = 0000 0110 1000 0000 (binary

            var messageLen = message.BitLength;

            var reversedBits = MsbLsbConversionHelpers.ReverseBitArrayBits(message.Bits);
            var normalizedBits = MsbLsbConversionHelpers.ReverseByteOrder(new BitString(reversedBits).ToBytes());

            message = new BitString(normalizedBits);
            var hex = message.ToHex();
            message = new BitString(hex, messageLen, false);

            return message;
        }
        #endregion Helpers
    }

}
