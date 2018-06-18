using NIST.CVP.Math;
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
                return X.Substring(start, end - start);
            }
            else
            {
                return X.Substring(start, X.BitLength - 1);
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
    }
}
