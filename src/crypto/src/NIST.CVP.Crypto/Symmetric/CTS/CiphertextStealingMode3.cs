using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 3 - final blocks always swapped (unless there's only a single block to work with. 
    /// </summary>
    public class CiphertextStealingMode3 : ICiphertextStealingTransform
    {
        public void Transform(byte[] ciphertext, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            if (numberOfBlocks == 1)
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