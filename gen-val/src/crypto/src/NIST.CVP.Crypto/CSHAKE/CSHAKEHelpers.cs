﻿using NIST.CVP.Math;
using NIST.CVP.Crypto.SHA3;

namespace NIST.CVP.Crypto.CSHAKE
{
    public class CSHAKEHelpers : SHA3DerivedHelpers
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
            var customization = StringToHex(customizationString);

            return FormatMessage(message, capacity, functionNameString, customization); 
        }

        public static BitString FormatMessage(BitString message, int capacity, string functionNameString, BitString customization)
        {
            var functionName = StringToHex(functionNameString);

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

            message = SafeConcatenation(message, BitString.Zeroes(2));

            return message;
        }
    }
}