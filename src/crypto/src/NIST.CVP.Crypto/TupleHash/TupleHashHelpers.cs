using NIST.CVP.Math;
using NIST.CVP.Crypto.SHA3;
using System.Collections.Generic;

namespace NIST.CVP.Crypto.TupleHash
{
    public class TupleHashHelpers : SHA3DerivedHelpers
    {
        /// <summary>
        /// Call this method to format correctly for TupleHash
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="digestSize">Desired output size</param>
        /// <param name="customizationString">Character string for customization</param>
        /// <param name="xof">Is it xof mode?</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(IEnumerable<BitString> tuples, int digestSize, string customizationString, bool xof)
        {
            BitString message = new BitString(0);

            foreach (BitString tuple in tuples)
            {
                message = BitString.ConcatenateBits(message, EncodeString(tuple));
            }

            if (xof)
            {
                message = BitString.ConcatenateBits(message, RightEncode(BitString.Zeroes(8)));
            }
            else
            {
                message = BitString.ConcatenateBits(message, RightEncode(IntToBitString(digestSize)));
            }

            return message;
        }
    }
}
