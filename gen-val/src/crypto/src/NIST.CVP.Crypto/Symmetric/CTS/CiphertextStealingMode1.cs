using NIST.CVP.Crypto.Common.Symmetric.CTS;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Math;
using System;

namespace NIST.CVP.Crypto.Symmetric.CTS
{
    /// <summary>
    /// Mode 1 - final blocks never swapped, number of bits "stolen" from second to last block are dropped from the ciphertext
    ///
    /// https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38a-add.pdf
    /// </summary>
    public class CiphertextStealingMode1 : ICiphertextStealingTransform
    {
        public void TransformCiphertext(byte[] outBuffer, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            if (numberOfBlocks == 1 || originalPayloadBitLength % engine.BlockSizeBits == 0)
            {
                return;
            }

            var bitsToAddForPadding = numberOfBlocks * engine.BlockSizeBits - originalPayloadBitLength;

            // When the "padded" block is the second to last block in the byte array,
            // We need to remove the padding bits from that block, and left shift
            // the last block to where the padding bits were removed.
	
            var blockBitsWithoutPadding = engine.BlockSizeBits - bitsToAddForPadding;
	
            // The "padded" block is the second to the last block
            var secondToLastBlockStartIndex = (numberOfBlocks - 2) * engine.BlockSizeBytes;
            var secondToLastBlockBytes = new byte[engine.BlockSizeBytes];
            Array.Copy(
                outBuffer, secondToLastBlockStartIndex, 
                secondToLastBlockBytes, 0, 
                engine.BlockSizeBytes);

            // The last block of outBuffer
            var lastBlockStartIndex = (numberOfBlocks - 1) * engine.BlockSizeBytes;
            var lastBlockBytes = new byte[engine.BlockSizeBytes];
            Array.Copy(
                outBuffer, lastBlockStartIndex, 
                lastBlockBytes, 0, 
                engine.BlockSizeBytes);
	
            // The last two blocks of outBuffer with padding removed
            var lastTwoBlocks = new BitString(secondToLastBlockBytes)
                .GetMostSignificantBits(blockBitsWithoutPadding) // second to last block, only the bits that aren't a part of padding
                .ConcatenateBits(new BitString(lastBlockBytes)) // last block
                .ToBytes();

            // Finally, copy the last two blocks onto the outBuffer,
            // starting at the secondToLastBlockStartIndex.
            Array.Copy(
                lastTwoBlocks, 0, 
                outBuffer, secondToLastBlockStartIndex, 
                lastTwoBlocks.Length);
            
            // The extra bits at the end of the byte array will be
            // automatically removed by the block cipher
        }

        public byte[] HandleFinalFullPayloadBlockDecryption(BitString payload, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            // When payload is not a multiple of the block size
            if (numberOfBlocks > 1 && payload.BitLength % engine.BlockSizeBits != 0)
            {
                var numberOfBitsToAdd = engine.BlockSizeBits - payload.BitLength % engine.BlockSizeBits;
                
                // Decrypt the last full payload block (in this case the last block)
                var decryptedLastBlockBuffer = new byte[engine.BlockSizeBytes];
                var lastBlock = new byte[engine.BlockSizeBytes];
                var lastBlockStartIndex = payload.BitLength / BitString.BITSINBYTE - engine.BlockSizeBytes;
                Array.Copy(payload.ToBytes(), lastBlockStartIndex, lastBlock, 0, engine.BlockSizeBytes);

                engine.ProcessSingleBlock(lastBlock, decryptedLastBlockBuffer, 0);

                var paddedPayload = payload
                    .GetMostSignificantBits(payload.BitLength - engine.BlockSizeBits)
                    .ConcatenateBits(new BitString(decryptedLastBlockBuffer).GetLeastSignificantBits(numberOfBitsToAdd))
                    .ConcatenateBits(payload.GetLeastSignificantBits(engine.BlockSizeBits))
                    .ToBytes();
                
                // //Swap the final two blocks
                // for (int i = 0; i < engine.BlockSizeBytes; i++)
                // {
                //     var secondToLastBlockIndex = (numberOfBlocks - 2) * engine.BlockSizeBytes + i;
                //     var lastBlockIndex = (numberOfBlocks - 1) * engine.BlockSizeBytes + i;
                //
                //     SwapBytesHelper.SwapBytes(paddedPayload, secondToLastBlockIndex, lastBlockIndex);
                // }

                return paddedPayload;
            }

            return payload.ToBytes();
        }
    }
}