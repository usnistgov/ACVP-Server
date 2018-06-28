using NIST.CVP.Math;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Crypto.CSHAKE;

namespace NIST.CVP.Crypto.ParallelHash
{
    public class ParallelHashHelpers : SHA3DerivedHelpers
    {
        /// <summary>
        /// Call this method to format correctly for ParallelHash
        /// </summary>
        /// <param name="message">BitString representation of message</param>
        /// <param name="cSHAKE">A cSHAKE module</param>
        /// <param name="capacity">Capacity</param>
        /// <param name="blockSize">The desired block size in bytes</param>
        /// <param name="customizationString">Character string for customization</param>
        /// <param name="xof">Is it xof mode?</param>
        /// <returns>Formatted message before calling Keccak</returns>
        public static BitString FormatMessage(BitString message, CSHAKEWrapper cSHAKE, int digestSize, int capacity, int blockSize, string customizationString, bool xof)
        {
            var numberOfBlocks = ((message.BitLength / 8) + blockSize - 1) / blockSize;

            var newMessage = LeftEncode(IntToBitString(blockSize));

            BitString substring;
            for (int i = 0; i < numberOfBlocks; i++)
            {
                substring = SubString(message, i * blockSize * 8, (i + 1) * blockSize * 8);
                newMessage = BitString.ConcatenateBits(newMessage, cSHAKE.HashMessage(substring, capacity, capacity));
            }

            newMessage = BitString.ConcatenateBits(newMessage, RightEncode(IntToBitString(numberOfBlocks)));

            if (xof)
            {
                newMessage = BitString.ConcatenateBits(newMessage, RightEncode(BitString.Zeroes(8)));
            }
            else
            {
                newMessage = BitString.ConcatenateBits(newMessage, RightEncode(IntToBitString(digestSize)));
            }

            return newMessage;
        }
    }
}
