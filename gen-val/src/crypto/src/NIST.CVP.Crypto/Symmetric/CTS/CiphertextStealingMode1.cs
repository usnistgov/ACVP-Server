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
            var blockBitsWithoutPadding = engine.BlockSizeBits - bitsToAddForPadding;
            
            var newPayload = new BitString(outBuffer);
            var newPayloadUpToPadding = newPayload
                // all the blocks up to the second to last block, which should only include the "non padded" bits
                .GetMostSignificantBits(engine.BlockSizeBits * (numberOfBlocks - 2) + blockBitsWithoutPadding);
            var finalBlock = newPayload.GetLeastSignificantBits(engine.BlockSizeBits);
            var newPayloadBytes = newPayloadUpToPadding
                .ConcatenateBits(finalBlock)
                // Adding zero bits in the LSB to not add significant bits when converting to a byte array.
                .ConcatenateBits(BitString.Zeroes(bitsToAddForPadding))
                .ToBytes();
            
            Array.Copy(newPayloadBytes, 0, outBuffer, 0, newPayloadBytes.Length);
        }

        public byte[] HandleFinalFullPayloadBlockDecryption(BitString payload, IBlockCipherEngine engine, int numberOfBlocks, int originalPayloadBitLength)
        {
            // When payload is not a multiple of the block size
            if (numberOfBlocks > 1 && payload.BitLength % engine.BlockSizeBits != 0)
            {
                var numberOfBitsToAdd = engine.BlockSizeBits - payload.BitLength % engine.BlockSizeBits;
                
                // Decrypt the last full payload block (in this case the last block)
                var decryptedLastBlockBuffer = new byte[engine.BlockSizeBytes];
                var lastBlock = payload.GetLeastSignificantBits(engine.BlockSizeBits).ToBytes();

                engine.ProcessSingleBlock(lastBlock, decryptedLastBlockBuffer, 0);

                var paddedPayload = payload
                    // The original payload minus the final full block 
                    .GetMostSignificantBits(payload.BitLength - engine.BlockSizeBits)
                    // Add the least significant bits of the decrypted last block to pad to a multiple of the block size
                    .ConcatenateBits(new BitString(decryptedLastBlockBuffer).GetLeastSignificantBits(numberOfBitsToAdd))
                    // Add the last block back onto the payload
                    .ConcatenateBits(payload.GetLeastSignificantBits(engine.BlockSizeBits));

                return paddedPayload.ToBytes();
            }

            return payload.ToBytes();
        }
    }
}