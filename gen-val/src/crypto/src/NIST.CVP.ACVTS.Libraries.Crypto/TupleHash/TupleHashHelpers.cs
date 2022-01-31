using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.TupleHash
{
    public static class TupleHashHelpers
    {
        /// <summary>
        /// Call this method to format correctly for TupleHash
        /// </summary>
        /// <param name="tuple">List of BitString representation of message</param>
        /// <param name="digestLength">Desired output size</param>
        /// <param name="xof">Is it xof mode?</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(IEnumerable<BitString> tuple, int digestLength, bool xof)
        {
            BitString message = new BitString(0);

            foreach (BitString element in tuple)
            {
                var encodedPiece = Sha3DerivedHelpers.EncodeString(element);
                message = Sha3DerivedHelpers.SafeConcatenation(message, encodedPiece);
            }

            if (xof)
            {
                var rightEncode = Sha3DerivedHelpers.RightEncode(BitString.Zeroes(8));
                message = Sha3DerivedHelpers.SafeConcatenation(message, rightEncode);
            }
            else
            {
                var rightEncode = Sha3DerivedHelpers.RightEncode(Sha3DerivedHelpers.IntToBitString(digestLength));
                message = Sha3DerivedHelpers.SafeConcatenation(message, rightEncode);
            }

            return message;
        }
    }
}
