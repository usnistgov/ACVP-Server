using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using System.Text;

namespace NIST.CVP.Crypto.SHA3
{
    public class CSHAKEHelpers
    {
        /// <summary>
        /// Call this method to format correctly for cSHAKE
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="capacity">Capacity</param>
        /// <param name="functionName">Character string representation of functionName</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(BitString message, int capacity, string functionNameString, string customizationString)
        {
            var functionName = StringToHex(functionNameString);
            var customization = StringToHex(customizationString);

            BitString bytepad;
            if (capacity == 256)
            {
                bytepad = Bytepad(BitString.ConcatenateBits(encode_string(functionName), encode_string(customization)), new BitString("A8"));
            }
            else   // capacity == 512
            {
                bytepad = Bytepad(BitString.ConcatenateBits(encode_string(functionName), encode_string(customization)), new BitString("88"));
            }

            message = BitString.ConcatenateBits(bytepad, message);

            message = BitString.ConcatenateBits(message, BitString.Zeroes(2));

            return message;
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString encode_string(BitString message)
        {
            var messageLen = message.BitLength / 4;
            var messageLenBitString = new BitString(new System.Numerics.BigInteger(messageLen));
            if (messageLen == 0)
            {
                messageLenBitString = BitString.Zeroes(8);
            }
            messageLenBitString = new BitString(MsbLsbConversionHelpers.ReverseBitArrayBits(messageLenBitString.Bits));
            messageLenBitString = left_encode(messageLenBitString);
            return BitString.ConcatenateBits(messageLenBitString, message);
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString left_encode(BitString message)
        {
            var messageLen = message.BitLength / 8;
            var messageLenBitString = new BitString(new System.Numerics.BigInteger(messageLen));
            return BitString.ConcatenateBits(messageLenBitString, message);
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString right_encode(BitString message)
        {
            var messageLen = message.BitLength / 8;
            var messageLenBitString = new BitString(new System.Numerics.BigInteger(messageLen));
            return BitString.ConcatenateBits(message, messageLenBitString);
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString Bytepad(BitString input, BitString w)
        {
            var z = BitString.ConcatenateBits(left_encode(w), input);
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

        // Converts char string to BitString
        private static BitString StringToHex(string words)
        {
            byte[] ba = Encoding.Default.GetBytes(words);
            return new BitString(ba);
        }
    }
}
