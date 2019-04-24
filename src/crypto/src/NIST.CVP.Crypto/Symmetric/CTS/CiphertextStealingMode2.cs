using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 2 - final blocks conditionally swapped.  When final block is not the full block size, swap them, otherwise leave as is.
    /// </summary>
    public class CiphertextStealingMode2 : ICiphertextStealingTransform
    {
        public void Transform(byte[] ciphertext, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            if (numberOfBlocks == 1 || originalPayloadBitLength % engine.BlockSizeBits == 0)
            {
                return;
            }

            for (int i = 0; i < engine.BlockSizeBytes; i++)
            {
                var secondToLastBlockIndex = (numberOfBlocks - 2) * engine.BlockSizeBytes + i;
                var lastBlockIndex = (numberOfBlocks - 1) * engine.BlockSizeBytes + i;

                SwapBytesHelper.SwapBytes(ciphertext, secondToLastBlockIndex, lastBlockIndex);
            }
        }
    }
}