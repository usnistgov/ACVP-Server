using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KMAC
{
    public class KmacHelpers : SHA3DerivedHelpers
    {
        /// <summary>
        /// Call this method to format correctly for KMAC
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="capacity">Capacity</param>
        /// <param name="customizationString">Character string for customization</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(BitString message, BitString key, int capacity, string customizationString, int macLength, bool xof)
        {
            var macLengthBitString = new BitString(new System.Numerics.BigInteger(macLength));

            BitString newMessage;
            if (capacity == 256)
            {
                newMessage = Bytepad(EncodeString(key), new BitString("A8"));   // "A8" is 164 (the rate)
            }
            else      // capacity == 512
            {
                newMessage = Bytepad(EncodeString(key), new BitString("88"));   // "88" is 136 (the rate)
            }

            if (xof)
            {
                newMessage = BitString.ConcatenateBits(newMessage, BitString.ConcatenateBits(message, RightEncode(BitString.Zeroes(8))));
            }
            else
            {
                newMessage = BitString.ConcatenateBits(newMessage, BitString.ConcatenateBits(message, RightEncode(macLengthBitString)));
            }

            return newMessage;
        }
    }
}
