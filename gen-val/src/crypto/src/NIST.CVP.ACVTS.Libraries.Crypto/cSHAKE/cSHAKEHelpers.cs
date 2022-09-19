using NIST.CVP.ACVTS.Libraries.Crypto.SHA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE
{
    public static class cSHAKEHelpers
    {
        /// <summary>
        /// Call this method to format correctly for cSHAKE
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="capacity">Capacity</param>
        /// <param name="functionNameString">Character string representation of functionName</param>
        /// <param name="customizationString">Character string for customization</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(BitString message, int capacity, string functionNameString, string customizationString)
        {
            var customization = Sha3DerivedHelpers.StringToHex(customizationString);

            return FormatMessage(message, capacity, functionNameString, customization);
        }

        public static BitString FormatMessage(BitString message, int capacity, string functionNameString, BitString customization)
        {
            var functionName = Sha3DerivedHelpers.StringToHex(functionNameString);

            BitString bytepad;
            if (capacity == 256)
            {
                bytepad = Sha3DerivedHelpers.Bytepad(BitString.ConcatenateBits(Sha3DerivedHelpers.EncodeString(functionName), Sha3DerivedHelpers.EncodeString(customization)), new BitString("A8")); // "A8" is 168 (the rate)
            }
            else   // capacity == 512
            {
                bytepad = Sha3DerivedHelpers.Bytepad(BitString.ConcatenateBits(Sha3DerivedHelpers.EncodeString(functionName), Sha3DerivedHelpers.EncodeString(customization)), new BitString("88")); // "88" is 136 (the rate)
            }

            message = BitString.ConcatenateBits(bytepad, message);

            message = Sha3DerivedHelpers.SafeConcatenation(message, BitString.Zeroes(2));

            return message;
        }
    }
}
