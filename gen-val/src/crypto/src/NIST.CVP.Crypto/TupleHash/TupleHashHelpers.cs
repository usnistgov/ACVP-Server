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
        /// <param name="digestLength">Desired output size</param>
        /// <param name="xof">Is it xof mode?</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(IEnumerable<BitString> tuple, int digestLength, bool xof)
        {
            BitString message = new BitString(0);

            foreach (BitString element in tuple)
            {
                message = SafeConcatenation(message, EncodeString(element));
            }

            if (xof)
            {
                message = SafeConcatenation(message, RightEncode(BitString.Zeroes(8)));
            }
            else
            {
                message = SafeConcatenation(message, RightEncode(IntToBitString(digestLength)));
            }

            return message;
        }
    }
}
