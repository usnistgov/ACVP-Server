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
                bytepad = Bytepad(BitString.ConcatenateBits(EncodeString(functionName), EncodeString(customization)), new BitString("A8")); // "A8" is 164 (the rate)
            }
            else   // capacity == 512
            {
                bytepad = Bytepad(BitString.ConcatenateBits(EncodeString(functionName), EncodeString(customization)), new BitString("88")); // "88" is 136 (the rate)
            }

            message = BitString.ConcatenateBits(bytepad, message);

            message = BitString.ConcatenateBits(message, BitString.Zeroes(2));

            return message;
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString EncodeString(BitString message)
        {
            var messageLen = message.BitLength;
            var messageLenBitString = new BitString(new System.Numerics.BigInteger(messageLen));
            if (messageLen == 0)
            {
                messageLenBitString = BitString.Zeroes(8);
            }
            messageLenBitString = LeftEncode(messageLenBitString);
            return BitString.ConcatenateBits(messageLenBitString, message);
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString LeftEncode(BitString message)
        {
            var messageLen = message.BitLength / 8;
            var messageLenBitString = new BitString(new System.Numerics.BigInteger(messageLen));
            return BitString.ConcatenateBits(messageLenBitString, message);
        }

        // Use this method for encoding strings for cSHAKE
        public static BitString RightEncode(BitString message)
        {
            var messageLen = message.BitLength / 8;
            var messageLenBitString = new BitString(new System.Numerics.BigInteger(messageLen));
            return BitString.ConcatenateBits(message, messageLenBitString);
        }

        // Use this method for encoding strings for cSHAKE
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

        // Converts char string to BitString
        private static BitString StringToHex(string words)
        {
            var ba = Encoding.ASCII.GetBytes(words);
            return new BitString(ba);
        }
    }
}
