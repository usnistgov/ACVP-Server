using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using System;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 1 - final blocks never swapped, number of bits "stolen" from second to last block are dropped from the outbuffer
    /// </summary>
    public class CiphertextStealingMode1 : ICiphertextStealingTransform
    {
        public void Transform(byte[] outBuffer, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            if (numberOfBlocks == 1 || originalPayloadBitLength % engine.BlockSizeBits == 0)
            {
                return;
            }

            var bitsToAddForPadding = numberOfBlocks * engine.BlockSizeBits - originalPayloadBitLength;
            var partialBlockBits = engine.BlockSizeBits - bitsToAddForPadding;

            var partialBlockBytes = partialBlockBits / 8;

            // remove the "padded amount" of bits from the second to last block, shifting the amount removed onto it
            Array.Copy(outBuffer, outBuffer.Length - (1 * engine.BlockSizeBytes),
                outBuffer, outBuffer.Length - (2 * engine.BlockSizeBytes) + partialBlockBytes,
                engine.BlockSizeBytes
            );

        }
    }
}