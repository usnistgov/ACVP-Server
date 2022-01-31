using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.ParallelHash
{
    public static class ParallelHashHelpers
    {
        /// <summary>
        /// Call this method to format correctly for ParallelHash
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="cSHAKE">A cSHAKE module</param>
        /// <param name="capacity">Capacity</param>
        /// <param name="blockSize">The desired block size in bytes</param>
        /// <param name="xof">Is it xof mode?</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(BitString message, CSHAKE.CSHAKE cSHAKE, int digestLength, int capacity, int blockSize, bool xof)
        {
            // 1. n = ⌈ (len(X)/8) / B ⌉.
            var numberOfBlocks = ((message.BitLength / 8) + blockSize - 1) / blockSize;

            // 2. z = left_encode(B).
            var newMessage = Sha3DerivedHelpers.LeftEncode(Sha3DerivedHelpers.IntToBitString(blockSize));

            // 3. for i = 0 to n−1:
            //    z = z || cSHAKE128(substring(X, i*B*8, (i+1)*B*8), 256, "", "").
            var strings = new List<BitString>();
            var hashFunction = new HashFunction { DigestLength = capacity, Capacity = capacity };
            for (var i = 0; i < numberOfBlocks; i++)
            {
                var substring = Sha3DerivedHelpers.SubString(message, i * blockSize * 8, (i + 1) * blockSize * 8);
                strings.Add(cSHAKE.HashMessage(hashFunction, substring, "").Digest);
            }

            for (int i = 0; i < numberOfBlocks; i++)
            {
                newMessage = BitString.ConcatenateBits(newMessage, strings[i]);
            }

            // 4. z = z || right_encode(n) || right_encode(L).
            newMessage = BitString.ConcatenateBits(newMessage, Sha3DerivedHelpers.RightEncode(Sha3DerivedHelpers.IntToBitString(numberOfBlocks)));

            if (xof)
            {
                newMessage = BitString.ConcatenateBits(newMessage, Sha3DerivedHelpers.RightEncode(BitString.Zeroes(8)));
            }
            else
            {
                newMessage = BitString.ConcatenateBits(newMessage, Sha3DerivedHelpers.RightEncode(Sha3DerivedHelpers.IntToBitString(digestLength)));
            }

            return newMessage;
        }
    }
}
