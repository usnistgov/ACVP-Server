using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.CTS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 3 - final blocks always swapped (unless there's only a single block to work with).
    ///
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38a-add.pdf 
    /// </summary>
    public class CiphertextStealingMode3 : ICiphertextStealingTransform
    {
        public void TransformCiphertext(byte[] ciphertext, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            TransformText(ciphertext, engine, numberOfBlocks, originalPayloadBitLength);
        }

        public byte[] HandleFinalFullPayloadBlockDecryption(BitString payload, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            // Decrypt the last full payload block (when there is more than one block)
            if (numberOfBlocks > 1)
            {
                var originalPayloadPaddedToBlockSize = payload.PadToModulus(engine.BlockSizeBits).ToBytes();

                // Decrypt the last full payload block (in this case the second to last block)
                var secondToLastBlock = new byte[engine.BlockSizeBytes];
                var secondToLastBlockStartIndex = (numberOfBlocks - 2) * engine.BlockSizeBytes;
                Array.Copy(originalPayloadPaddedToBlockSize, secondToLastBlockStartIndex, secondToLastBlock, 0, engine.BlockSizeBytes);

                var decryptedSecondToLastBlockBuffer = new byte[engine.BlockSizeBytes];

                engine.ProcessSingleBlock(secondToLastBlock, decryptedSecondToLastBlockBuffer, 0);

                var decryptedBlock = new BitString(decryptedSecondToLastBlockBuffer);

                // Pad the payload to the nearest multiple of the block size using the last B−M bits of block cipher decryption of the second-to-last ciphertext block.
                var amountToPad = (engine.BlockSizeBits - payload.BitLength % engine.BlockSizeBits);
                if (amountToPad > 0)
                {
                    payload = payload.ConcatenateBits(BitString.Substring(decryptedBlock, 0, amountToPad));
                }

                var payloadBytes = payload.ToBytes();
                TransformText(payloadBytes, engine, numberOfBlocks, originalPayloadBitLength);

                payload = new BitString(payloadBytes);
                return payload.ToBytes();
            }

            return payload.ToBytes();
        }

        private void TransformText(byte[] text, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            // No blocks to swap when there's only a single block to work with
            if (numberOfBlocks == 1)
            {
                return;
            }

            // Swap the final two blocks
            for (int i = 0; i < engine.BlockSizeBytes; i++)
            {
                var secondToLastBlockIndex = (numberOfBlocks - 2) * engine.BlockSizeBytes + i;
                var lastBlockIndex = (numberOfBlocks - 1) * engine.BlockSizeBytes + i;

                SwapBytesHelper.SwapBytes(text, secondToLastBlockIndex, lastBlockIndex);
            }
        }
    }
}
