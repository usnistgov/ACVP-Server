using NIST.CVP.ACVTS.Libraries.Crypto.SHA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KMAC
{
    public static class KmacHelpers
    {
        /// <summary>
        /// Call this method to format correctly for KMAC
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="capacity">Capacity</param>
        /// <param name="customizationString">Character string for customization</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(BitString message, BitString key, int capacity, int macLength, bool xof)
        {
            var macLengthBitString = new BitString(new System.Numerics.BigInteger(macLength));

            BitString newMessage;
            if (capacity == 256)
            {
                newMessage = Sha3DerivedHelpers.Bytepad(Sha3DerivedHelpers.EncodeString(key), new BitString("A8"));   // "A8" is 164 (the rate)
            }
            else      // capacity == 512
            {
                newMessage = Sha3DerivedHelpers.Bytepad(Sha3DerivedHelpers.EncodeString(key), new BitString("88"));   // "88" is 136 (the rate)
            }

            if (xof)
            {
                var concatenatedMessageEncode = Sha3DerivedHelpers.SafeConcatenation(message, Sha3DerivedHelpers.RightEncode(BitString.Zeroes(8)));
                newMessage = BitString.ConcatenateBits(newMessage, concatenatedMessageEncode);
            }
            else
            {
                var concatenatedMessageEncode = Sha3DerivedHelpers.SafeConcatenation(message, Sha3DerivedHelpers.RightEncode(macLengthBitString));
                newMessage = BitString.ConcatenateBits(newMessage, concatenatedMessageEncode);
            }

            return newMessage;
        }
    }
}
