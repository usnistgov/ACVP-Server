using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 2 - final blocks conditionally swapped.  When final block is not the full block size, swap them, otherwise leave as is.
    ///
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38a-add.pdf
    /// </summary>
    public class CiphertextStealingMode2 : ICiphertextStealingTransform
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
                // When at the block size, nothing was swapped
                if (payload.BitLength % engine.BlockSizeBits == 0)
                {
                    return payload.ToBytes();
                }
                
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
            // When the payload meets the block size, do nothing
            if (numberOfBlocks == 1 || originalPayloadBitLength % engine.BlockSizeBits == 0)
            {
                return;
            }

            // When the text does not match a mod of the block size, swap the final two blocks
            for (int i = 0; i < engine.BlockSizeBytes; i++)
            {
                var secondToLastBlockIndex = (numberOfBlocks - 2) * engine.BlockSizeBytes + i;
                var lastBlockIndex = (numberOfBlocks - 1) * engine.BlockSizeBytes + i;

                SwapBytesHelper.SwapBytes(text, secondToLastBlockIndex, lastBlockIndex);
            }
        }
    }
}